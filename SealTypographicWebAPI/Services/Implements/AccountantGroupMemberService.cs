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
        public AccountantGroupMemberServiceExtension(SealTypographicDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        ///<inheritdoc /> 
        public AccountantGroupMembers GetMembers(AccountantGroupMemberSearch accountantGroupMemberSearch)
        {
            AccountantGroupMembers accountantGroupMembers = new();
            List<AccountantGroupMember> accountantMembers = new();     
                        
            // Todo 調整寫法 
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
        public ResponseViewModel UpdateGroupMembers(AccountantGroupMemberForm accountantGroupMemberForm)
        {
            ResponseViewModel response = new();
            
            List<int> noDataAccountantIds = new();
            AccountantGroup? accountantGroup = dbContext.AccountantGroups                                                
                                                .FirstOrDefault(x => x.Id == accountantGroupMemberForm.AccountantGroupId);

            AccountantGroup defaultAccountantGroup = dbContext.AccountantGroups.Single(x => x.Id == 1);
                    
            if (accountantGroup != null)
            {                
                foreach (int accountantId in accountantGroupMemberForm.JoinAccountantIds)
                {
                    Accountant? accountant = dbContext.Accountants
                                            .Include(x => x.AccountantGroups)
                                            .FirstOrDefault(x => x.Id == accountantId);
                    if (accountant != null)
                    {
                        //如果已加入群組則不動作
                        if(!accountant.AccountantGroups.Contains(accountantGroup))
                        {
                            accountant.AccountantGroups.Add(accountantGroup);
                        }
                        
                        //如果此會計師有包含預設群組就移除
                        if (accountant.AccountantGroups.Any(x => x.Id == 1))
                        {
                            accountant.AccountantGroups.Remove(defaultAccountantGroup);
                        }                                        
                    }
                    else
                    {
                        noDataAccountantIds.Add(accountantId);
                    }
                }

                foreach (int accountantId in accountantGroupMemberForm.LeaveAccountantIds)
                {                    
                    Accountant? accountant = dbContext.Accountants
                                            .Include(x => x.AccountantGroups)
                                            .FirstOrDefault(x => x.Id == accountantId);
                    if (accountant != null)
                    {                        
                        accountant.AccountantGroups.Remove(accountantGroup);

                        //如果此會計師沒有任何群組則加入預設群組
                        if (!accountant.AccountantGroups.Any())
                        {
                            accountant.AccountantGroups.Add(defaultAccountantGroup);
                        }                        
                    }
                    else
                    {
                        noDataAccountantIds.Add(accountantId);
                    }
                }

                if (noDataAccountantIds.Count == 0)
                {
                    dbContext.SaveChanges();
                    response.Success();
                }
                else
                {
                    string errorMessage = string.Format($"Accountant no data Ids: ");
                    foreach (int noDataAccountantId in noDataAccountantIds)
                    {
                        errorMessage += $"{noDataAccountantId},";
                    }
                    response.AccountantNoData();
                    response.Message = errorMessage;
                }
            }
            else
            {
                response.AccountantGroupNoData();
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
                //如果有包含預設群組就拿掉
                if(accountant.AccountantGroups.Any(x => x.Id == 1))
                {
                    accountant.AccountantGroups.Remove(dbContext.AccountantGroups.Single(x => x.Id == 1));
                }

                if(!accountant.AccountantGroups.Any(x => x.Id == accountantGroupId))
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
