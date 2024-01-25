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
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Models.LogReport.OperationLog;

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
        public AccountantDetailResponse GetDetail(int accountantId, int userId = 1)
        {
            logger.LogInformation("GetDetail input accountantId: {@accountantId} userId {@userId}", accountantId, userId);

            AccountantDetailResponse accountantResponse = new();

            try
            {
                AccountantViewModel? accountantDetailViewModel = dbContext.Accountants
                                                                .Include(accountant => accountant.AccountantGroups)                                                                                                                                                                                   
                                                                .Where(accountant => accountant.Id == accountantId)
                                                                .ProjectTo<AccountantViewModel>(configurationProvider)
                                                                .FirstOrDefault();

                if (accountantDetailViewModel != null)
                {
                    accountantResponse.AccountantDetailViewModel = accountantDetailViewModel;
                    accountantResponse.Success();
                    logReportService.SaveOperationLog(mapper.Map<OperationLogSave>(accountantDetailViewModel));
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
        public AccountantPaginateViewModel GetPaginate(AccountantSearch accountantSearch, bool isTypographicUse, int userId = 1)
        {
            logger.LogInformation("GetPaginate input {@accountantSearch} isTypographicUse: {@isTypographicUse} userId {@userId}"
                ,accountantSearch, isTypographicUse, userId);

            AccountantPaginateViewModel accountantPaginatesViewModels = new();
            int companyId = 1;

            try
            {
                IQueryable<Accountant> accountantQuery = dbContext.Accountants
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

                if (!string.IsNullOrWhiteSpace(accountantSearch.AccountantGroupNumber))
                {
                    accountantQuery = accountantQuery.Where(accountant => accountant.GroupAccountants.First().AccountantGroup.Code == accountantSearch.AccountantGroupNumber);
                }

                accountantQuery = accountantQuery.OrderBy(accountant => accountant.Id);

                if (accountantQuery.Any())
                {
                    //取得該頁            
                    accountantPaginatesViewModels.ViewModels = accountantQuery
                                                                .Skip((accountantSearch.PageNumber - 1) * accountantSearch.PageSize)
                                                                .Take(accountantSearch.PageSize)
                                                                .ProjectTo<AccountantViewModelWithCreateDate>(configurationProvider)
                                                                .ToList();

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
        public AccountantCreateResponse New(AccountantForm accountantForm, int userId = 1)
        {
            logger.LogInformation("New input {@accountantForm} userId: {@userId}", accountantForm, userId);

            AccountantCreateResponse accountantCreateResponse = new();            
            int companyId = 1;

            try
            {
                //尋找公司並與會計師關聯
                Company? companyQuery = dbContext.Companys.Include(x => x.Accountants).FirstOrDefault(x => x.Id == companyId);

                if (companyQuery != null)
                {
                    //驗證編號是否重複
                    List<string> accountantCodeQuery = companyQuery.Accountants.Where
                                                        (
                                                            x => x.Code == accountantForm.AccountantNumber
                                                            && x.DeleteStatus == DeleteStatus.No
                                                        ).Select(x => x.Code).ToList();

                    if (!accountantCodeQuery.Any())
                    {
                        Accountant dbAccountant = mapper.Map<Accountant>(accountantForm);
                        dbAccountant.AccountantGroups = dbContext.AccountantGroups.Where(x => x.Id == accountantForm.AccountantGroupId).ToList();
                        BaseInputAccountant(dbAccountant, true, userId);
                        companyQuery.Accountants.Add(dbAccountant);
                        dbContext.SaveChanges();

                        if (dbAccountant != null)
                        {
                            //回傳剛建立的會計師基本資料 使建立會計師簽印找到該ID
                            accountantCreateResponse.AccountantId = dbAccountant.Id;
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
                logger.LogInformation("New dbError {@dbError}", ex.Message);
            }
            catch (Exception ex)
            {
                accountantCreateResponse.Error();
                logger.LogInformation("New error {@error}", ex.Message);
            }            

            return accountantCreateResponse;
        }

        ///<inheritdoc />      
        public ResponseViewModel Update(AccountantUpdateForm accountantFormUpdate, int userId = 1)
        {
            logger.LogInformation("Update input {@accountantFormUpdate} userId {@userId}", accountantFormUpdate, userId);

            ResponseViewModel response = new();
            
            try
            {
                Accountant? accountantQuery = dbContext.Accountants.Include(x => x.AccountantGroups)
                                                .FirstOrDefault(x => x.Id == accountantFormUpdate.Id);

                if (accountantQuery != null)
                {
                    mapper.Map(accountantFormUpdate, accountantQuery);
                    //TODO 更新群組功能之後可能要拔掉
                    if(!accountantQuery.AccountantGroups.Any(x => x.Id == accountantFormUpdate.AccountantGroupId))
                    {                        
                        accountantQuery.AccountantGroups.Add(dbContext.AccountantGroups.Single(x => x.Id == accountantFormUpdate.AccountantGroupId));
                    }

                    BaseInputAccountant(accountantQuery, false, userId);
                    dbContext.SaveChanges();
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
                logger.LogInformation("Update dbError {@dbError}", ex.Message);
            }
            catch (Exception ex) 
            {
                response.Error();
                logger.LogInformation("Update error {@error}", ex.Message);
            }
            
            return response;
        }

        ///<inheritdoc />     
        public ResponseViewModel Delete(int accountantId, int userId = 1)
        {
            logger.LogInformation("Delete input accountantId: {@accountantId} userId {@userId}", accountantId, userId);

            ResponseViewModel response = new();
            
            try
            {
                logger.LogInformation("Delete output {@output}", response);
            }
            catch (DbUpdateException ex)
            {
                response.DbError();
                logger.LogInformation("GetPaginate dbError {@dbError}", ex.Message);
            }
            catch (Exception ex)
            {
                response.Error();
                logger.LogInformation("GetPaginate error {@error}", ex.Message);
            }

            Accountant? accountantQuery = dbContext.Accountants.Find(accountantId);

            if (accountantQuery != null)
            {
                accountantQuery.DeleteStatus = DeleteStatus.Yes;
                BaseInputAccountant(accountantQuery, false, userId);
                dbContext.SaveChanges();
                response.Success();
            }
            else
            {
                response.DeleteAccountantNoData();
            }
            return response;
        }

        /// <summary>
        /// 信頭資料新增修改時基本資料輸入
        /// </summary>
        /// <param name="accountant">DB上的客戶資料</param>
        /// <param name="isCreate">確認是否新增的動作</param>
        /// <param name="userid">使用者ID</param>
        private static void BaseInputAccountant(Accountant accountant, bool isCreate, int userid)
        {
            if (isCreate)
            {
                accountant.CreateUserId = userid;
                accountant.CreateDate = DateTime.Now;
                accountant.DeleteStatus = DeleteStatus.No;
            }
            else
            {
                accountant.UpdateUserId = userid;
                accountant.UpdateDate = DateTime.Now;
            }
        }             
    }    
}