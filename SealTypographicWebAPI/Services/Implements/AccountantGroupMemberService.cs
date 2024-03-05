using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.AccountantGroupMember;
using SealTypographicWebAPI.Utils;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DBEntities.Consts;
using DBEntities;
using DBEntities.Entities.AccountantModels;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 會計師群組成員管理
    /// </summary>
    public class AccountantGroupMemberService : IAccountantGroupMemberService
    {
        private readonly SealTypographicDbContext dbContext;        
        private readonly AutoMapper.IConfigurationProvider configurationProvider;
        private readonly ILogger<AccountantGroupMemberService> logger;

        /// <summary>
        /// 注入DB、ResponseService
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>                
        public AccountantGroupMemberService(SealTypographicDbContext dbContext, IMapper mapper, ILogger<AccountantGroupMemberService> logger)
        {
            this.dbContext = dbContext;            
            this.configurationProvider = mapper.ConfigurationProvider;
            this.logger = logger;
        }

        ///<inheritdoc /> 
        public async Task<AccountantGroupMembers> GetMembers(AccountantGroupMemberSearch accountantGroupMemberSearch, bool isGroupMember, int userId = 1)
        {
            logger.LogInformation("GetMembers input {@accountantGroupMemberSearch} isGroupMember: {@isGroupMember} userId: {@userId}", accountantGroupMemberSearch, isGroupMember, userId);

            AccountantGroupMembers accountantGroupMembers = new();

            try
            {
                IQueryable<Accountant> accountantQuery = dbContext.Accountants.Where(x => x.DeleteStatus == DeleteStatus.No);

                if (isGroupMember)
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
                    accountantGroupMembers.Members = await PageUtil.SetPaginateViewModelAsync<Accountant, AccountantGroupMember>
                                                    (accountantQuery, configurationProvider, accountantGroupMemberSearch.PageNumber, accountantGroupMemberSearch.PageSize);

                    PageUtil.SetPaginate(accountantGroupMembers, accountantGroupMemberSearch.PageNumber, accountantGroupMemberSearch.PageSize, accountantQuery.Count());
                    accountantGroupMembers.Success();
                }
                else
                {
                    accountantGroupMembers.DbNoData();
                }
                logger.LogInformation("GetMembers output {@output}", accountantGroupMembers);
            }
            catch (Exception ex) 
            {
                accountantGroupMembers.Error();
                logger.LogInformation("GetMembers error {@error}", ex.Message);
            }            

            return accountantGroupMembers;
        }
      

        ///<inheritdoc /> 
        public async Task<ResponseViewModel> UpdateGroupMembers(AccountantGroupMemberForm accountantGroupMemberForm, int userId = 1)
        {
            logger.LogInformation("UpdateGroupMembers input {@accountantGroupMemberForm} userId {@userId}", accountantGroupMemberForm, userId);

            ResponseViewModel response = new();            
            List<int> noDataAccountantIds = new();

            try
            {
                AccountantGroup? accountantGroup = dbContext.AccountantGroups
                                               .FirstOrDefault(x => x.Id == accountantGroupMemberForm.AccountantGroupId);

                AccountantGroup defaultAccountantGroup = dbContext.AccountantGroups.Single(x => x.Id == DefaultConsts.AccountantGroupId);

                if (accountantGroup != null)
                {
                    //加入群組的成員的處理
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

                    //離開群組的成員處理
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
                        await dbContext.SaveChangesAsync();
                        response.Success();
                    }
                    else
                    {
                        response.AccountantNoData();
                        response.Message = string.Format($"Accountant no data Ids: {string.Join(", ",noDataAccountantIds)}");
                    }
                }
                else
                {
                    response.AccountantGroupNoData();
                }
                logger.LogInformation("UpdateGroupMembers output {@output}", response);
            }
            catch (Exception ex)
            {
                response.Error();
                logger.LogInformation("UpdateGroupMembers error {@error}", ex.Message);
            }               

            return response;
        }        
    }
}
