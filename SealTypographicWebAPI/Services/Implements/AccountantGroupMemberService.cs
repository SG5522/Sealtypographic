using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.AccountantGroupMember;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Utils;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DBEntities;
using DBEntities.Consts;


namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 會計師群組成員管理
    /// </summary>
    public class AccountantGroupMemberServiceExtension : IAccountantGroupMemberService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly IMapper mapper;

        /// <summary>
        /// 注入DB、ResponseService
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>                
        public AccountantGroupMemberServiceExtension(SealTypographicDbContext dbContext,IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        ///<inheritdoc /> 
        public AccountantGroupMembers GetMembers(AccountantGroupMemberSearch accountantGroupMemberSearch)
        {
            AccountantGroupMembers accountantGroupMembers = new();
            List<AccountantGroupMember> accountantMembers = new();            
            IQueryable<Accountant> accountantQuery = dbContext.Accountants
                                                    .Where
                                                    (
                                                        accountant => 
                                                        accountant.AccountantGroups.First().Id == accountantGroupMemberSearch.AccountantGroupId
                                                        && accountant.DeleteStatus == DeleteStatus.No                                                        
                                                    )
                                                    .OrderBy(accountant => accountant.Id);

            if (accountantQuery.Any())
            {
                //取得該頁            
                List<Accountant> accountants = accountantQuery
                                            .Skip((accountantGroupMemberSearch.PageNumber - 1) * accountantGroupMemberSearch.PageSize)
                                            .Take(accountantGroupMemberSearch.PageSize)
                                            .ToList();

                foreach (Accountant accountant in accountants)
                {
                    accountantMembers.Add(new()
                    {
                        Id = accountant.Id,
                        AccountantNumber = accountant.Code,
                        Name = accountant.Name,
                    });
                }
                accountantGroupMembers.Members = accountantMembers;
                accountantGroupMembers.PageNumber = accountantGroupMemberSearch.PageNumber;
                accountantGroupMembers.PageSize = accountantGroupMemberSearch.PageSize;
                //計算總頁數                
                accountantGroupMembers.TotalPage = TotalPageUtil.GetTotalPage(accountantQuery.Count(), accountantGroupMemberSearch.PageSize);
                accountantGroupMembers.TotalCount = accountantQuery.Count();
                                
            }
            accountantGroupMembers.Success();

            return accountantGroupMembers;
        }

        ///<inheritdoc /> 
        public NotThisGroupMember GetNotThisGroupMember(NotThisGroupMemberSearch notThisGroupMemberSearch)
        {
            NotThisGroupMember notThisGroupMember = new();
            List<AccountantViewModel> accountantViewModels = new();
            
            IQueryable<Accountant>? accountantQuery = dbContext.Accountants                                                        
                                                        .Where(accountant => accountant.AccountantGroups.Any(x => x.Id != notThisGroupMemberSearch.AccountantGroupId)
                                                        && accountant.DeleteStatus == DeleteStatus.No);
                                                
                                                
            if(notThisGroupMemberSearch.KeyWord != null)
            {
                accountantQuery = accountantQuery.Where
                                (
                                    accountant => accountant.Code.Contains(notThisGroupMemberSearch.KeyWord)
                                    || accountant.Name.Contains(notThisGroupMemberSearch.KeyWord)
                                );
            }

            if (accountantQuery.Any())
            {
                //取得該頁            
                List<Accountant> accountants =  accountantQuery
                                                .Skip((notThisGroupMemberSearch.PageNumber - 1) * notThisGroupMemberSearch.PageSize)
                                                .Take(notThisGroupMemberSearch.PageSize)
                                                .ToList();

                foreach (Accountant accountant in accountants)
                {
                    AccountantViewModel accountantViewModel = mapper.Map<AccountantViewModel>(accountant);
                    //accountantViewModel.AccountantGroupName = accountant.AccountantGroup.Name;
                    accountantViewModels.Add(accountantViewModel);
                }
                notThisGroupMember.AccountantViewModels = accountantViewModels;
                notThisGroupMember.PageNumber = notThisGroupMemberSearch.PageNumber;
                notThisGroupMember.PageSize = notThisGroupMemberSearch.PageSize;
                //計算總頁數
                notThisGroupMember.TotalPage = TotalPageUtil.GetTotalPage(accountantQuery.Count(), notThisGroupMemberSearch.PageSize);
                notThisGroupMember.TotalCount = accountantQuery.Count();                            
            }
            notThisGroupMember.Success();

            return notThisGroupMember;
        }

        ///<inheritdoc />
        public ResponseViewModel UpdateGroup(AccountantGroupChangeForm accountantGroupChangeForm)
        {
            ResponseViewModel response = new();

            if(ChangeGroupMember(accountantGroupChangeForm.Id, accountantGroupChangeForm.AccountantGroupId))    
            {
                dbContext.SaveChanges();
                response.Success();
            }
            else
            {                
                response.UpdateAccountantNoData();
            }
            
            return response;
        }

        ///<inheritdoc /> 
        public ResponseViewModel ChangeNotTheGroupMember(AccountantGroupMemberForm accountantGroupMemberForm)
        {
            ResponseViewModel response = new();
            
            foreach (int accountantId in accountantGroupMemberForm.AccountantIds)
            {
                if(!ChangeGroupMember(accountantId, accountantGroupMemberForm.AccountantGroupId))
                {
                    if(response.Message == null)
                    {
                        response.Message = $"Update accountant no data accountantId:";
                    }
                    response.Code = (int)ResponseCode.UpdateAccountantNoData;
                    response.Message += $" {accountantId},";
                }
            }

            if(response.Message == null)
            {
                dbContext.SaveChanges();
                response.Success();
            }
            return response;
        }

        private bool ChangeGroupMember(int accountantId, int accountantGroupId)
        {
            bool result;
            Accountant? accountant = dbContext.Accountants.Include(x => x.AccountantGroups)
                                    .FirstOrDefault(x => x.Id  == accountantId);
            if (accountant != null)
            {
                if(accountant.AccountantGroups.Any(x => x.Id == accountantGroupId))
                {
                    accountant.AccountantGroups.Add(dbContext.AccountantGroups.Single(x => x.Id == accountantGroupId));
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                result = false;
            }
            return result;
        }
    }
}
