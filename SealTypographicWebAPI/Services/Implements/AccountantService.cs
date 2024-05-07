using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Utils;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DBEntities.Consts;
using AutoMapper.QueryableExtensions;
using DBEntities.Entities.AccountantModels;
using DBEntities.Entities;
using DBEntities;
using CommonLib.Models;
using SealTypographicWebAPI.Models.LogReport.OperationLog;
using DBEntities.Utils;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 會計師資料管理
    /// </summary>
    public class AccountantService : IAccountantService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly IMapper mapper;
        private readonly AutoMapper.IConfigurationProvider configurationProvider;
        private readonly ILogger<AccountantService> logger;
        private readonly ILogReportService logReportService;

        /// <summary>
        /// 注入DB與Mapper
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        /// <param name="logReportService"></param>
        public AccountantService(SealTypographicDbContext dbContext, IMapper mapper, ILogger<AccountantService> logger, ILogReportService logReportService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            configurationProvider = mapper.ConfigurationProvider;
            this.logger = logger;
            this.logReportService = logReportService;
        }

        ///<inheritdoc />
        public async Task<AccountantDetailResponse> GetDetail(int accountantId, UserInfo userInfo)
        {
            logger.LogInformation("GetDetail input accountantId: {@accountantId} userId {@userId}", accountantId, userInfo.UserId);

            AccountantDetailResponse accountantResponse = new();

            try
            {
                AccountantViewModel? accountantDetailViewModel = await dbContext.Accountants
                                                                .Include(accountant => accountant.AccountantGroups)                                                                                                                                                                                   
                                                                .Where(accountant => accountant.Id == accountantId)
                                                                .ProjectTo<AccountantViewModel>(configurationProvider)
                                                                .FirstOrDefaultAsync();

                if (accountantDetailViewModel != null)
                {
                    accountantResponse.AccountantDetailViewModel = accountantDetailViewModel;
                    accountantResponse.Success();
                    await logReportService.SaveOperationLog
                            (
                                mapper.Map<OperationLogSave>(accountantDetailViewModel), 
                                userInfo.UserName, 
                                $"{userInfo.FirstName}{userInfo.LastName}"
                            );
                }
                else
                {
                    accountantResponse.DbNoData();
                }
                LogModel<AccountantDetailResponse> log = new()
                {
                    Data = accountantResponse
                };
                logger.LogInformation("GetDetail output {@output}", accountantResponse);
            }
            catch (Exception ex) 
            {
                accountantResponse.Error();
                logger.LogError("GetDetail error {@error}", ex.Message);
            }

            return accountantResponse;
        }

        ///<inheritdoc />
        public async Task<AccountantPaginateViewModel> GetPaginate(AccountantSearch accountantSearch, bool isTypographicUse, int userId = 1)
        {
            logger.LogInformation("GetPaginate input {@accountantSearch} isTypographicUse: {@isTypographicUse} userId {@userId}"
                ,accountantSearch, isTypographicUse, userId);

            AccountantPaginateViewModel accountantPaginatesViewModels = new();
            int companyId = 1;

            try
            {
                IQueryable<Accountant> accountantQuery = dbContext.Accountants
                                                        .Include(x => x.AccountantSignGroups)
                                                        .Where
                                                        (
                                                            accountant => accountant.Company.Id == companyId
                                                            && accountant.DeleteStatus == DeleteStatus.No                                                            
                                                        );

                if (isTypographicUse)
                {
                    accountantQuery = accountantQuery.Where(accountant => accountant.AccountantSignGroups.Any(x => x.ReviewStatus == ReviewStatus.Approval));
                }

                if (!string.IsNullOrWhiteSpace(accountantSearch.KeyWord))
                {
                    accountantQuery = accountantQuery.Where
                    (
                        accountant =>
                        accountant.Code.ToLower().Contains(accountantSearch.KeyWord.ToLower())
                        || accountant.Name.ToLower().Contains(accountantSearch.KeyWord.ToLower())
                    );
                }

                if (accountantSearch.AccountantGroupId != 0)
                {
                    accountantQuery = accountantQuery.Where(accountant => accountant.AccountantGroups.Any(x => x.Id == accountantSearch.AccountantGroupId));
                }

                accountantQuery = accountantQuery.OrderBy(accountant => accountant.Id);                

                if (accountantQuery.Any())
                {
                    //取得該頁     
                    accountantPaginatesViewModels.ViewModels = await PageUtil.SetPaginateViewModelAsync<Accountant, AccountantViewModelWithCreateDate>
                                                                (accountantQuery, configurationProvider, accountantSearch.PageNumber, accountantSearch.PageSize);

                    PageUtil.SetPaginate(accountantPaginatesViewModels, accountantSearch.PageNumber, accountantSearch.PageSize, accountantQuery.Count());
                    accountantPaginatesViewModels.Success();
                }
                else
                {
                    accountantPaginatesViewModels.DbNoData();
                }
                logger.LogInformation("GetPaginate output {@output}", accountantPaginatesViewModels);
            }
            catch (Exception ex) 
            {
                accountantPaginatesViewModels.Error();
                logger.LogInformation("GetPaginate error {@error}", ex.Message);
            }            

            return accountantPaginatesViewModels;            
        }


        ///<inheritdoc />
        public async Task<AccountantCreateResponse> New(AccountantForm accountantForm, int userId = 1, int companyId = 1)
        {
            logger.LogInformation("New input {@accountantForm} userId: {@userId}", accountantForm, userId);

            AccountantCreateResponse accountantCreateResponse = new();                        

            try
            {
                //確認公司是否存在
                Company? companyQuery = dbContext.Companys.Include(x => x.Accountants).FirstOrDefault(x => x.Id == companyId);

                if (companyQuery != null)
                {
                    //驗證編號是否重複
                    bool isAccountantCodeDuplicate = dbContext.Accountants.Any
                                                    (                                                            
                                                        x => x.Code == accountantForm.AccountantNumber
                                                        && x.Company.Id == companyId
                                                        && x.DeleteStatus == DeleteStatus.No
                                                    );

                    if (!isAccountantCodeDuplicate)
                    {
                        Accountant newAccountant = mapper.Map<Accountant>(accountantForm);
                        IList<AccountantGroup> accountantGroups = dbContext.AccountantGroups
                                                                .Where(x => accountantForm.AccountantGroupIds.Contains(x.Id))
                                                                .ToList();

                        newAccountant.AccountantGroups = accountantGroups;
                        InputUtil.Set(newAccountant, userId, true);
                        companyQuery.Accountants.Add(newAccountant);

                        await dbContext.SaveChangesAsync();

                        if (newAccountant != null)
                        {
                            //回傳剛建立的會計師基本資料 使建立會計師簽印找到該ID
                            accountantCreateResponse.AccountantId = newAccountant.Id;
                            accountantCreateResponse.Success();
                        }
                        else
                        {
                            accountantCreateResponse.CreateAccountantFailed();
                        }
                    }
                    else
                    {
                        accountantCreateResponse.AccountantNumberRepeat();
                    }
                }
                logger.LogInformation("New output {@output}", accountantCreateResponse);
            }
            catch (DbUpdateException ex)
            {
                accountantCreateResponse.DbError();
                logger.LogError("New dbError {@dbError}", ex.InnerException);
            }
            catch (Exception ex)
            {
                accountantCreateResponse.Error();
                logger.LogError("New error {@error}", ex.Message);
            }            

            return accountantCreateResponse;
        }

        ///<inheritdoc />      
        public async Task<ResponseViewModel> Update(AccountantUpdateForm accountantFormUpdate, int userId = 1)
        {
            logger.LogInformation("Update input {@accountantFormUpdate} userId {@userId}", accountantFormUpdate, userId);

            ResponseViewModel response = new();
            
            try
            {
                Accountant? accountantQuery = dbContext.Accountants.Include(x => x.AccountantGroups)
                                                .FirstOrDefault(x => x.Id == accountantFormUpdate.Id);

                if (accountantQuery != null)
                {
                    IList<AccountantGroup> accountantGroups = dbContext.AccountantGroups
                                                                .Where(x => accountantFormUpdate.AccountantGroupIds.Contains(x.Id))
                                                                .ToList();

                    //更新會計師群組
                    accountantQuery.AccountantGroups = accountantGroups;

                    //更新會計師群組以外的資料
                    mapper.Map(accountantFormUpdate, accountantQuery);
                                        
                    InputUtil.Set(accountantQuery, userId, false);
                    await dbContext.SaveChangesAsync();
                    response.Success();
                }
                else
                {
                    response.UpdateAccountantNoData();
                }
                logger.LogInformation("Update output {@output}", response);
            }
            catch (DbUpdateException ex)
            {
                response.DbError();
                logger.LogError("Update dbError {@dbError}", ex.InnerException);
            }
            catch (Exception ex) 
            {
                response.Error();
                logger.LogError("Update error {@error}", ex.Message);
            }
            
            return response;
        }

        ///<inheritdoc />     
        public async Task<ResponseViewModel> Delete(int accountantId, int userId = 1)
        {
            logger.LogInformation("Delete input accountantId: {@accountantId} userId {@userId}", accountantId, userId);

            ResponseViewModel response = new();
            
            try
            {
                Accountant? accountantQuery = dbContext.Accountants.Find(accountantId);

                if (accountantQuery != null)
                {
                    accountantQuery.DeleteStatus = DeleteStatus.Yes;
                    InputUtil.Set(accountantQuery, userId, false);
                    await dbContext.SaveChangesAsync();
                    response.Success();
                }
                else
                {
                    response.DeleteAccountantNoData();
                }

                logger.LogInformation("Delete output {@output}", response);
            }
            catch (DbUpdateException ex)
            {
                response.DbError();
                logger.LogError("GetPaginate dbError {@dbError}", ex.InnerException);
            }
            catch (Exception ex)
            {
                response.Error();
                logger.LogError("GetPaginate error {@error}", ex.Message);
            }
            
            return response;
        }        
    }    
}