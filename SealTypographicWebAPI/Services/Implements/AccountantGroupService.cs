using AutoMapper;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.AccountantGroup;
using SealTypographicWebAPI.Utils;
using DBEntities.Consts;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using DBEntities;
using DBEntities.Entities.AccountantModels;
using DBEntities.Utils;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 會計師群組管理
    /// </summary>
    public class AccountantGroupService : IAccountantGroupService
    {
        private readonly SealTypographicDbContext dbContext;        
        private readonly IMapper mapper;
        private readonly AutoMapper.IConfigurationProvider configurationProvider;
        private readonly ILogger<AccountantGroupService> logger;

        /// <summary>
        /// 注入DB、ResponseService
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>        
        public AccountantGroupService(SealTypographicDbContext dbContext,IMapper mapper, ILogger<AccountantGroupService> logger)
        {
            this.dbContext = dbContext;            
            this.mapper = mapper;
            configurationProvider = mapper.ConfigurationProvider;
            this.logger = logger;
        }

        ///<inheritdoc />
        public AccountantGroupList GetAll()
        {
            AccountantGroupList accountantGroupList = new();

            try
            {
                IQueryable<AccountantGroupViewModel> accountantGroupDatas = dbContext.AccountantGroups
                                                                            .Where(accountantGroup => accountantGroup.DeleteStatus == DeleteStatus.No)
                                                                            .ProjectTo<AccountantGroupViewModel>(configurationProvider);

                if (accountantGroupDatas.Any())
                {
                    accountantGroupList.AccountantGroupDatas = accountantGroupDatas.ToList();
                    accountantGroupList.Success();
                }
                else
                {
                    accountantGroupList.AccountantGroupNoData();
                }
                logger.LogInformation("GetAll output {@output}", accountantGroupList);
            }
            catch (Exception ex) 
            {
                accountantGroupList.Error();
                logger.LogInformation("GetAll error {@error}", ex.Message);
            }

            return accountantGroupList;
        }

        ///<inheritdoc />
        public async Task<AccountantGroupResponse> GetData(int accountantGroupId, int userId = 1)
        {
            logger.LogInformation("GetData input accountantGroupId: {@accountantGroupId} userId: {@userId}", accountantGroupId, userId);

            AccountantGroupResponse accountantGroupResponse = new();

            try
            {
                AccountantGroupViewModel? accountantGroup = await dbContext.AccountantGroups
                                                            .Where(accountantGroup => accountantGroup.Id == accountantGroupId)
                                                            .ProjectTo<AccountantGroupViewModel>(configurationProvider)
                                                            .FirstOrDefaultAsync(accountantGroup => accountantGroup.Id == accountantGroupId);

                if (accountantGroup != null)
                {
                    accountantGroupResponse.AccountantGroupData = accountantGroup;
                    accountantGroupResponse.Success();
                }
                else
                {
                    accountantGroupResponse.AccountantGroupNoData();
                }
                logger.LogInformation("GetAll output {@output}", accountantGroupResponse);

            }
            catch (Exception ex)
            {
                accountantGroupResponse.Error();
                logger.LogInformation("GetData error {@error}", ex.Message);
            }            

            return accountantGroupResponse;
        }

        ///<inheritdoc />
        public async Task<AccountantGroupPaginateViewModel> GetPaginate(AccountantGroupSearch accountantGroupSearch, int userId = 1)
        {
            logger.LogInformation("GetPaginate input {@accountantGroupSearch} userId: {@userId}", accountantGroupSearch, userId);

            AccountantGroupPaginateViewModel accountantGroupResponses = new();
            int companyId = 1;

            try
            {
                IQueryable<AccountantGroup> accountantGroupsQuery = dbContext.AccountantGroups.Where
                                                                (
                                                                    x => x.DeleteStatus == DeleteStatus.No
                                                                    && x.Company.Id == companyId
                                                                );

                if (!string.IsNullOrWhiteSpace(accountantGroupSearch.GroupName))
                {
                    accountantGroupsQuery = accountantGroupsQuery.Where
                                            (
                                                accountantGroup =>
                                                accountantGroup.Name.Contains(accountantGroupSearch.GroupName)
                                                || accountantGroup.Code.ToLower().Contains(accountantGroupSearch.GroupName.ToLower())
                                            );
                }
                accountantGroupsQuery.OrderBy(accountantGroup => accountantGroup.Code);

                if (accountantGroupsQuery.Any())
                {
                    //取得該頁
                    accountantGroupResponses.AccountantGroups =  await PageUtil.SetPaginateViewModelAsync<AccountantGroup, AccountantGroupViewModel>
                                                                (accountantGroupsQuery, configurationProvider, accountantGroupSearch.PageNumber, accountantGroupSearch.PageSize);

                    PageUtil.SetPaginate(accountantGroupResponses, accountantGroupSearch.PageNumber, accountantGroupSearch.PageSize, accountantGroupsQuery.Count());
                    accountantGroupResponses.Success();
                }
                else
                {
                    accountantGroupResponses.AccountantGroupNoData();
                }
                logger.LogInformation("GetPaginate output {@output}", accountantGroupResponses);
            }
            catch (Exception ex)
            {
                accountantGroupResponses.Error();
                logger.LogInformation("GetPaginate error {@error}", ex.Message);
            }            

            return accountantGroupResponses;
        }

        ///<inheritdoc />
        public async Task<ResponseViewModel> New(AccountantGroupForm accountantGroupForm, int userId = 1)
        {
            logger.LogInformation("New input {@accountantGroupForm} userId: {@userId}", accountantGroupForm, userId);

            ResponseViewModel response = new();
            int companyId = 1;            

            try
            {
                //確認編號是否重複
                AccountantGroup? accountantGroupQuery = dbContext.AccountantGroups
                                                        .FirstOrDefault
                                                        (
                                                            accountantGroup =>
                                                            accountantGroup.Code == accountantGroupForm.AccountantGroupNumber
                                                            && accountantGroup.DeleteStatus == DeleteStatus.No
                                                            && accountantGroup.Company.Id == companyId
                                                        );

                if (accountantGroupQuery == null)
                {
                    AccountantGroup accountantGroup = mapper.Map<AccountantGroup>(accountantGroupForm);                    
                    accountantGroup.Company = dbContext.Companys.Single(x => x.Id == companyId);                    
                    InputUtil.Set(accountantGroup, userId, true);
                    dbContext.AccountantGroups.Add(accountantGroup);                    
                    await dbContext.SaveChangesAsync();
                    response.Success();
                }
                else
                {
                    response.CreateAccountantGroupNumberRepeat();
                }
                logger.LogInformation("New output {@output}", response);
            }
            catch (Exception ex)
            {
                response.Error();
                logger.LogInformation("New error {@error}", ex.Message);
            }
            
            return response;
        }

        ///<inheritdoc />
        public async Task<ResponseViewModel> Update(AccountantGroupUpdateForm accountantGroupFormUpdate, int userId = 1)
        {
            logger.LogInformation("Update input {@accountantGroupFormUpdate} userId: {@userId}", accountantGroupFormUpdate, userId);

            ResponseViewModel response = new();
            
            try
            {
                AccountantGroup? accountantGroupQuery = dbContext.AccountantGroups.Find(accountantGroupFormUpdate.Id);

                if (accountantGroupQuery != null)
                {
                    mapper.Map(accountantGroupFormUpdate, accountantGroupQuery);
                    InputUtil.Set(accountantGroupQuery, userId, false);
                    accountantGroupQuery.UpdateDate = DateTime.Now;
                    accountantGroupQuery.UpdateUserId = userId;

                    await dbContext.SaveChangesAsync();
                    response.Success();
                }
                else
                {
                    response.UpdateAccountantGroupNoData();
                }
                logger.LogInformation("Update output {@output}", response);
            }
            catch (Exception ex)
            {
                response.Error();
                logger.LogInformation("Update error {@error}", ex.Message);
            }
            
            return response;
        }

        ///<inheritdoc />       
        public async Task<ResponseViewModel> Delete(int accountantGroupId, int userId = 1)
        {
            logger.LogInformation("Delete input accountantGroupId: {@accountantGroupId} userId: {@userId}", accountantGroupId, userId);

            ResponseViewModel response = new();

            try
            {                
                AccountantGroup? accountantGroup = dbContext.AccountantGroups.FirstOrDefault(x => x.Id == accountantGroupId);                

                if (accountantGroup != null)
                {                    
                    dbContext.Remove(accountantGroup);
                    await dbContext.SaveChangesAsync();
                    response.Success();
                }
                else
                {
                    response.DeleteAccountantGroupNoData();
                }
                logger.LogInformation("Delete output {@output}", response);
            }
            catch (DbUpdateException ex)
            {
                response.DbNoData();
                logger.LogInformation("Delete dberror {@dberror}", ex.Message);
            }
            catch (Exception ex)
            {
                response.Error();
                logger.LogInformation("Delete error {@error}", ex.Message);
            }
            
            return response;
        }
    }
}
