using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.AccountantGroupMember;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Utils;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DBEntities;
using DBEntities.Consts;
using AutoMapper.QueryableExtensions;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 會計師群組成員管理
    /// </summary>
    public class AccountantGroupMemberServiceExtension : IAccountantGroupMemberService
    {
        private readonly SealTypographicDbContext dbContext;        
        private readonly AutoMapper.IConfigurationProvider configurationProvider;

        /// <summary>
        /// 注入DB、ResponseService
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>                
        public AccountantGroupMemberServiceExtension(SealTypographicDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;            
            this.configurationProvider = mapper.ConfigurationProvider;
        }

        ///<inheritdoc /> 
        public AccountantGroupMembers GetMembers(AccountantGroupMemberSearch accountantGroupMemberSearch, bool isGroup)
        {
            AccountantGroupMembers accountantGroupMembers = new();

            IQueryable<Accountant> accountantQuery = dbContext.Accountants.Where(x => x.DeleteStatus == DeleteStatus.No);
                                                                     
            if(isGroup)
            {
                accountantQuery = accountantQuery.Where(x => x.AccountantGroups.Any(x => x.Id == accountantGroupMemberSearch.AccountantGroupId));
            }
            else
            {
                accountantQuery = accountantQuery.Where(x => !x.AccountantGroups.Any(x => x.Id == accountantGroupMemberSearch.AccountantGroupId));
            }

            accountantQuery = accountantQuery.OrderBy(x => x.Id);

            if (accountantQuery.Any())
            {
                //取得該頁            
                accountantGroupMembers.Members = accountantQuery                                                
                                                .Skip((accountantGroupMemberSearch.PageNumber - 1) * accountantGroupMemberSearch.PageSize)
                                                .Take(accountantGroupMemberSearch.PageSize)
                                                .ProjectTo<AccountantGroupMember>(configurationProvider)
                                                .ToList();

                PageUtil.GetPageData(accountantGroupMembers, accountantGroupMemberSearch.PageNumber, accountantGroupMemberSearch.PageSize, accountantQuery.Count());
                accountantGroupMembers.Success();
            }            

            return accountantGroupMembers;
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
                        //如果此會計師有包含預設群組就移除
                        if (accountant.AccountantGroups.Any(x => x.Id == 1))
                        {
                            accountant.AccountantGroups.Remove(defaultAccountantGroup);
                        }
                        //如果已加入群組則不動作
                        if (!accountant.AccountantGroups.Contains(accountantGroup))
                        {
                            accountant.AccountantGroups.Add(accountantGroup);
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
