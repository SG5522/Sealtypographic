using AutoMapper;
using CommonLib.Enums;
using CommonLib.Extensions;
using CommonLib.Models;
using DBEntities;
using DBEntities.Consts;
using DBEntities.Entities.AccountantModels;
using DBEntities.Entities.TypographicModels;
using DJKeycloakLib.Models.BaseModel;
using DJKeycloakLib.Models.Group;
using DJKeycloakLib.Models.User;
using DJKeycloakLib.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SealTypographicWebAPI.Config;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Extensions;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.LogReport.AccountantList;
using SealTypographicWebAPI.Models.LogReport.AccountantMember;
using SealTypographicWebAPI.Models.LogReport.AccountantSignLog;
using SealTypographicWebAPI.Models.LogReport.CustomerSealEventLog;
using SealTypographicWebAPI.Models.LogReport.OperationLog;
using SealTypographicWebAPI.Models.LogReport.TypographicReport;
using SealTypographicWebAPI.Models.LogReport.UserMember;
using SealTypographicWebAPI.Models.MongoDBModel;
using SealTypographicWebAPI.Utils;


namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 報表管理Service
    /// 紀錄操作(主要為查詢)、客戶印鑑異動、會計師簽印異動
    /// </summary>
    public class LogReportService : ILogReportService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly IAdminService adminService;
        private readonly ILogger<LogReportService> logger;
        private readonly IMapper mapper;
        private readonly AutoMapper.IConfigurationProvider configurationProvider;
        private IMongoCollection<OperationLog> operationLog;
        private IMongoCollection<CustomerSealEventLog> customerSealEventLog;
        private IMongoCollection<AccountantSignEventLog> accountantSignEventLog;


        /// <summary>
        /// 建置
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="adminService"></param>        
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="options"></param>
        public LogReportService(SealTypographicDbContext dbContext, IAdminService adminService, ILogger<LogReportService> logger, IMapper mapper, IOptionsMonitor<LogDatabaseOptions> options)
        {
            this.dbContext = dbContext;
            this.adminService = adminService;
            this.logger = logger;
            this.mapper = mapper;
            configurationProvider = mapper.ConfigurationProvider;
            //MongoDb連線            
            MongoClient mongoClient = new(options.CurrentValue.ConnectionString);
            IMongoDatabase mongoDatabase = mongoClient.GetDatabase(options.CurrentValue.DatabaseName);
            Init(mongoDatabase);
        }

        /// <summary>
        /// mongodb的初始化
        /// </summary>
        /// <param name="mongoDatabase"></param>
        private void Init(IMongoDatabase mongoDatabase)
        {
            //建立TimeSeriesCollection
            CreateTimeSeriesCollection(mongoDatabase, LogDbCollectionNames.OperationLog);
            CreateTimeSeriesCollection(mongoDatabase, LogDbCollectionNames.CustomerSealEventLog);
            CreateTimeSeriesCollection(mongoDatabase, LogDbCollectionNames.AccountantSignEventLog);
            //取得Collection
            operationLog = mongoDatabase.GetCollection<OperationLog>(LogDbCollectionNames.OperationLog);
            customerSealEventLog = mongoDatabase.GetCollection<CustomerSealEventLog>(LogDbCollectionNames.CustomerSealEventLog);
            accountantSignEventLog = mongoDatabase.GetCollection<AccountantSignEventLog>(LogDbCollectionNames.AccountantSignEventLog);
        }

        /// <summary>
        /// 建立TimeSeriesCollection。
        /// 先確認是否資料庫有該Collection，
        /// 沒有才依TimeSeriesCollection方式建立。
        /// </summary>
        /// <param name="mongoDatabase"></param>
        /// <param name="collectionName"></param>
        private static void CreateTimeSeriesCollection(IMongoDatabase mongoDatabase, string collectionName)
        {
            ListCollectionsOptions listCollectionsOptions = new()
            {
                Filter = new BsonDocument
                {
                    { "name", collectionName },
                }
            };

            if (!mongoDatabase.ListCollections(listCollectionsOptions).Any())
            {
                mongoDatabase.CreateCollection(collectionName, new() { TimeSeriesOptions = new TimeSeriesOptions("DateTime") });
            }
        }

        /// <summary>
        /// 取得動作類別名稱
        /// </summary>
        /// <returns></returns>
        public ActionTypeResponse GetActionType()
        {
            ActionTypeResponse actionTypeResponse = new();
            try
            {
                foreach (ActionType actionType in (ActionType[])Enum.GetValues(typeof(ActionType)))
                {
                    ActionTypeViewModel uploadTypeViewModel = new()
                    {
                        ActionType = actionType,
                        Name = actionType.GetDescription()
                    };
                    actionTypeResponse.ViewModels.Add(uploadTypeViewModel);
                }
                actionTypeResponse.Success();
                logger.LogInformation("GetUploadType output {@Output}", actionTypeResponse);
            }
            catch (Exception ex)
            {
                actionTypeResponse.Error();
                logger.LogError("GetUploadType error {@Error}", ex.Message);
            }

            return actionTypeResponse;
        }

        // 注意 MongoDB 不支援使用 "!" 運算子來判定 null 值，因此在存取其屬性之前，需要額外確保 x.Data 不為 null。
        // 下述有類似Data可以Null的情況請參照此作法

        /// <summary>
        /// 操作紀錄分頁列表
        /// </summary>
        /// <returns></returns>
        public OperationLogPaginate GetOperationLogPaginate(OperationLogSearch operationLogSearch, bool isFullPageOut = false)
        {
            OperationLogPaginate operationLogPaginate = new();

            logger.LogInformation("OperationLogPaginate input operationLogSearch: {@operationLogSearch}", operationLogSearch);

            IQueryable<OperationLog> operationLogQuery = operationLog.AsQueryable().Where
                                                        (
                                                            x => x.LogTimestamp >= operationLogSearch.StartDate
                                                            && x.LogTimestamp <= operationLogSearch.EndDate
                                                        );

            if (operationLogSearch.ActionType != null)
            {
                operationLogQuery = operationLogQuery.Where(x => x.Data != null
                                                            && x.Data.ActionType == operationLogSearch.ActionType.Value);
            }

            if (!string.IsNullOrWhiteSpace(operationLogSearch.UserKeyWord))
            {
                operationLogQuery = operationLogQuery.Where(x => x.UserId.ToLower().Contains(operationLogSearch.UserKeyWord.ToLower())
                                                            || x.UserName.ToLower().Contains(operationLogSearch.UserKeyWord.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(operationLogSearch.TargetKeyWord))
            {
                operationLogQuery = operationLogQuery.Where(x => x.Data != null &&
                                                                (
                                                                    x.Data.CustomerName.ToLower().Contains(operationLogSearch.TargetKeyWord.ToLower())
                                                                    || x.Data.AccountantName.ToLower().Contains(operationLogSearch.TargetKeyWord.ToLower())
                                                                )
                                                            );
            }

            if (operationLogQuery.Any())
            {
                if (isFullPageOut)
                {
                    operationLogPaginate.ViewModels = PageUtil.SetPaginateViewModel<OperationLog, OperationLogViewModel>
                                                        (
                                                            operationLogQuery,
                                                            configurationProvider
                                                        );
                }
                else
                {
                    operationLogPaginate.ViewModels = PageUtil.SetPaginateViewModel<OperationLog, OperationLogViewModel>
                                                        (
                                                            operationLogQuery,
                                                            configurationProvider,
                                                            operationLogSearch.PageNumber,
                                                            operationLogSearch.PageSize
                                                        );

                    PageUtil.SetPaginate(operationLogPaginate, operationLogSearch.PageNumber, operationLogSearch.PageSize, operationLogQuery.Count());
                }

                operationLogPaginate.Success();
            }
            else
            {
                operationLogPaginate.DbNoData();
            }

            return operationLogPaginate;
        }

        /// <summary>
        /// 客戶印鑑異動分頁列表
        /// </summary>
        /// <returns></returns>
        public CustomerSealEventLogPaginate GetCustomerSealEventLogPaginate(CustomerSealEventLogSearch customerSealEventLogSearch, TypographyType typographyType, bool isFullPageOut = false)
        {
            CustomerSealEventLogPaginate customerSealEventLogPaginate = new();

            logger.LogInformation("OperationLogPaginate input operationLogSearch: {@operationLogSearch}", customerSealEventLogSearch);

            IQueryable<CustomerSealEventLog> customerSealEventLogQuery = customerSealEventLog.AsQueryable().Where
                                                                        (
                                                                            x => x.LogTimestamp >= customerSealEventLogSearch.StartDate
                                                                            && x.LogTimestamp <= customerSealEventLogSearch.EndDate
                                                                            && x.Data != null
                                                                            && x.Data.TypographyType == typographyType
                                                                        );

            if (customerSealEventLogSearch.ReviewStatus != null)
            {
                customerSealEventLogQuery = customerSealEventLogQuery.Where(
                                                                                x => x.Data != null
                                                                                && x.Data.ReviewStatus == customerSealEventLogSearch.ReviewStatus.Value
                                                                            );
            }

            if (!string.IsNullOrWhiteSpace(customerSealEventLogSearch.UserKeyWord))
            {
                customerSealEventLogQuery = customerSealEventLogQuery
                                            .Where(x => x.UserId.ToLower().Contains(customerSealEventLogSearch.UserKeyWord.ToLower())
                                            || x.UserName.ToLower().Contains(customerSealEventLogSearch.UserKeyWord.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(customerSealEventLogSearch.CustomerKeyWord))
            {
                customerSealEventLogQuery = customerSealEventLogQuery
                                            .Where(x => x.Data != null &&
                                            (
                                                x.Data.CustomerCode.ToLower().Contains(customerSealEventLogSearch.CustomerKeyWord.ToLower())
                                                || x.Data.CustomerName.ToLower().Contains(customerSealEventLogSearch.CustomerKeyWord.ToLower())
                                            ));
            }

            if (customerSealEventLogQuery.Any())
            {
                if (isFullPageOut)
                {
                    customerSealEventLogPaginate.ViewModels = PageUtil.SetPaginateViewModel<CustomerSealEventLog, CustomerSealEventLogViewModel>
                                                                (
                                                                    customerSealEventLogQuery,
                                                                    configurationProvider
                                                                );
                }
                else
                {
                    customerSealEventLogPaginate.ViewModels = PageUtil.SetPaginateViewModel<CustomerSealEventLog, CustomerSealEventLogViewModel>
                                                                (
                                                                    customerSealEventLogQuery,
                                                                    configurationProvider,
                                                                    customerSealEventLogSearch.PageNumber,
                                                                    customerSealEventLogSearch.PageSize
                                                                );
                }


                PageUtil.SetPaginate(customerSealEventLogPaginate, customerSealEventLogSearch.PageNumber, customerSealEventLogSearch.PageSize, customerSealEventLogQuery.Count());

                customerSealEventLogPaginate.Success();
            }
            else
            {
                customerSealEventLogPaginate.DbNoData();
            }

            return customerSealEventLogPaginate;
        }

        /// <summary>
        /// 會計師簽印異動分頁列表
        /// </summary>
        /// <returns></returns>
        public AccountantSignEventLogPaginate GetAccountantSignEventLogPaginate(AccountantSignEventLogSearch accountantSignEventLogSearch, bool isFullPageOut = false)
        {
            AccountantSignEventLogPaginate accountantSignEventLogPaginate = new();

            logger.LogInformation("OperationLogPaginate input operationLogSearch: {@operationLogSearch}", accountantSignEventLogSearch);

            IQueryable<AccountantSignEventLog> accountantSignEventLogQuery = accountantSignEventLog.AsQueryable().Where
                                                                            (
                                                                                x => x.LogTimestamp >= accountantSignEventLogSearch.StartDate
                                                                                && x.LogTimestamp <= accountantSignEventLogSearch.EndDate
                                                                            );

            if (accountantSignEventLogSearch.ReviewStatus != null)
            {
                //MongoDb 沒辦法使用!判別null不比對所以需要加上x.Data != null
                accountantSignEventLogQuery = accountantSignEventLogQuery.Where(x => x.Data != null
                                                                                && x.Data.ReviewStatus == accountantSignEventLogSearch.ReviewStatus.Value);
            }

            if (!string.IsNullOrWhiteSpace(accountantSignEventLogSearch.UserKeyword))
            {
                accountantSignEventLogQuery = accountantSignEventLogQuery.Where(x => x.UserId.ToLower().Contains(accountantSignEventLogSearch.UserKeyword.ToLower())
                                                            || x.UserName.ToLower().Contains(accountantSignEventLogSearch.UserKeyword.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(accountantSignEventLogSearch.AccountantKeyword))
            {
                accountantSignEventLogQuery = accountantSignEventLogQuery.Where(x => x.Data != null &&
                                                                (
                                                                    x.Data.AccountantCode.ToLower().Contains(accountantSignEventLogSearch.AccountantKeyword.ToLower())
                                                                    || x.Data.AccountantName.ToLower().Contains(accountantSignEventLogSearch.AccountantKeyword.ToLower())
                                                                )
                                                            );
            }

            if (accountantSignEventLogQuery.Any())
            {
                if (isFullPageOut)
                {
                    accountantSignEventLogPaginate.ViewModels = PageUtil.SetPaginateViewModel<AccountantSignEventLog, AccountantSignEventLogViewModel>
                                            (
                                                accountantSignEventLogQuery,
                                                configurationProvider
                                            );
                }
                else
                {
                    accountantSignEventLogPaginate.ViewModels = PageUtil.SetPaginateViewModel<AccountantSignEventLog, AccountantSignEventLogViewModel>
                                                                (
                                                                    accountantSignEventLogQuery,
                                                                    configurationProvider,
                                                                    accountantSignEventLogSearch.PageNumber,
                                                                    accountantSignEventLogSearch.PageSize
                                                                );
                }

                PageUtil.SetPaginate(accountantSignEventLogPaginate, accountantSignEventLogSearch.PageNumber, accountantSignEventLogSearch.PageSize, accountantSignEventLogQuery.Count());

                accountantSignEventLogPaginate.Success();
            }
            else
            {
                accountantSignEventLogPaginate.DbNoData();
            }

            return accountantSignEventLogPaginate;
        }

        /// <summary>
        /// 取得排版紀錄
        /// </summary>
        /// <param name="customerTypoReportSearch"></param>
        /// <param name="typographyType"></param>
        /// <param name="userId"></param>
        /// <param name="isFullPageOut"></param>
        /// <returns></returns>
        public TypographicReportPaginate GetTypographicReport(TypographicReportSearch customerTypoReportSearch, TypographyType typographyType, int userId = 1, bool isFullPageOut = false)
        {
            logger.LogInformation("GetTypographicReport input customerTypoReportSearch: {@customerTypoReportSearch} typographyType: {@typographyType} userId: {@userId}"
                , customerTypoReportSearch, typographyType, userId);

            TypographicReportPaginate customerTypoReportPaginate = new();
            try
            {
                IQueryable<TypographicPDF> typographicPDFQuery = dbContext.TypographicPDFs
                                                                .Include(x => x.Customer)
                                                                .Include(x => x.UpdateUser)
                                                                .Where(
                                                                            x => x.DeleteStatus == DeleteStatus.No
                                                                            && x.TypographyType == typographyType
                                                                            && x.ReviewStatus == ReviewStatus.Approval
                                                                            && x.UpdateDate >= customerTypoReportSearch.StartDate
                                                                            && x.UpdateDate <= customerTypoReportSearch.EndDate
                                                                      );

                if (!string.IsNullOrEmpty(customerTypoReportSearch.UserKeyWord))
                {
                    typographicPDFQuery = typographicPDFQuery.Where(x => x.UpdateUser != null
                                                                        && (
                                                                            (x.UpdateUser.LastName != null
                                                                            && x.UpdateUser.LastName.Contains(customerTypoReportSearch.UserKeyWord))
                                                                            || x.UpdateUser.UserName.Contains(customerTypoReportSearch.UserKeyWord)
                                                                        ));
                }

                if (!string.IsNullOrEmpty(customerTypoReportSearch.CustomerKeyWord))
                {
                    typographicPDFQuery = typographicPDFQuery.Where(x => x.Customer.Name.Contains(customerTypoReportSearch.CustomerKeyWord)
                                                                    || x.Customer.Code.Contains(customerTypoReportSearch.CustomerKeyWord));
                }

                typographicPDFQuery = typographicPDFQuery.OrderBy(x => x.Id);

                if (typographicPDFQuery.Any())
                {
                    if (isFullPageOut)
                    {
                        customerTypoReportPaginate.ViewModels = PageUtil.SetPaginateViewModel<TypographicPDF, TypographicReportViewModel>
                                                                (
                                                                    typographicPDFQuery,
                                                                    configurationProvider
                                                                );
                    }
                    else
                    {
                        customerTypoReportPaginate.ViewModels = PageUtil.SetPaginateViewModel<TypographicPDF, TypographicReportViewModel>
                                                                (
                                                                    typographicPDFQuery,
                                                                    configurationProvider,
                                                                    customerTypoReportSearch.PageNumber,
                                                                    customerTypoReportSearch.PageSize
                                                                );
                    }

                    PageUtil.SetPaginate(customerTypoReportPaginate, customerTypoReportSearch.PageNumber, customerTypoReportSearch.PageSize, typographicPDFQuery.Count());
                }
                else
                {
                    customerTypoReportPaginate.DbNoData();
                }

                logger.LogInformation("GetTypographicReport output {@customerTypoReportPaginate}", customerTypoReportPaginate);
            }
            catch (Exception ex)
            {
                logger.LogError("GetTypographicReport error {@error}", ex.Message);
            }
            return customerTypoReportPaginate;
        }

        /// <summary>
        /// 取得會計師成員分頁列表
        /// </summary>
        /// <param name="accountantMemberSearch">會計師成員查詢</param>
        /// <param name="userId">使用者Id</param>
        /// <param name="isFullPageOut"></param>        
        /// <returns></returns>
        public AccountantMemberPaginate GetAccountantMemberPaginate(AccountantMemberSearch accountantMemberSearch, int userId = 1, bool isFullPageOut = false)
        {
            AccountantMemberPaginate accountantMemberPaginate = new();

            logger.LogInformation("GetAccountantMemberPaginate input accountantMemberSearch: {@accountantMemberSearch} userId: {@userId}"
                , accountantMemberSearch, userId);
            int companyId = 1;

            try
            {
                IQueryable<Accountant> accountantQuery = dbContext.Accountants
                                                                .Include(x => x.AccountantGroups)
                                                                .Where
                                                                (
                                                                    accountant => accountant.Company.Id == companyId
                                                                    && accountant.DeleteStatus == DeleteStatus.No
                                                                );


                if (!string.IsNullOrWhiteSpace(accountantMemberSearch.Keyword))
                {
                    accountantQuery = accountantQuery.Where
                    (
                        groupAccountant =>
                        groupAccountant.Code.ToLower().Contains(accountantMemberSearch.Keyword.ToLower())
                            || groupAccountant.Name.ToLower().Contains(accountantMemberSearch.Keyword.ToLower())
                    );
                }

                if (accountantMemberSearch.AccountantGroupId != 0)
                {
                    accountantQuery = accountantQuery.Where(accountant => accountant.AccountantGroups.Any(group => group.Id == accountantMemberSearch.AccountantGroupId));
                }


                // 根據會計師的 ID 進行分組，並選擇每組中的第一個元素
                accountantQuery = accountantQuery.OrderBy(accountant => accountant.Id);

                if (accountantQuery.Any())
                {
                    if (isFullPageOut)
                    {
                        accountantMemberPaginate.ViewModels = PageUtil.SetPaginateViewModel<Accountant, AccountantMemberViewModel>
                                                                (
                                                                    accountantQuery,
                                                                    configurationProvider
                                                                );
                    }
                    else
                    {
                        accountantMemberPaginate.ViewModels = PageUtil.SetPaginateViewModel<Accountant, AccountantMemberViewModel>
                                                                (
                                                                    accountantQuery,
                                                                    configurationProvider,
                                                                    accountantMemberSearch.PageNumber,
                                                                    accountantMemberSearch.PageSize
                                                                );
                    }

                    PageUtil.SetPaginate(accountantMemberPaginate, accountantMemberSearch.PageNumber, accountantMemberSearch.PageSize, accountantQuery.Count());
                    accountantMemberPaginate.Success();
                }
                else
                {
                    accountantMemberPaginate.DbNoData();
                }
                logger.LogInformation("GetPaginate output {@output}", accountantMemberPaginate);
            }
            catch (Exception ex)
            {
                logger.LogError("GetAccountantMemberPaginate error {@error}", ex.Message);
            }
            return accountantMemberPaginate;
        }

        /// <summary>
        /// 取得UserMember
        /// </summary>
        /// <param name="userMemberSearch">使用者成員查詢</param>
        /// <param name="isFullPageOut">是否全部輸出</param>
        /// <returns></returns>
        public async Task<UserMemberPaginate> GetUserMember([FromQuery] UserMemberSearch userMemberSearch, bool isFullPageOut = false)
        {
            UserMemberPaginate userMemberPaginate = new();

            ResponseModel<int> usersCount = await adminService.GetUsersCount(true, null, null, true, null,
                null, null, null, userMemberSearch.GetSanitizedQuery());

            //確定有資料後取得群組資料
            if (usersCount.Data > 0)
            {                
                IList<UserRepresentation> userRepresentations = new List<UserRepresentation>();

                // 局部函數，封裝 FindUsers 邏輯
                async Task<bool> FetchUserRepresentations(int? offset, int? limit)
                {
                    var response = await adminService.FindUsers(true, null, null, true, null,
                        null, null, null, userMemberSearch.GetSanitizedQuery(), offset, limit);

                    if (response.Code == (int)KeycloakResponseCode.Success && response.Data != null)
                    {
                        userRepresentations.AddRange(response.Data);
                        return true;
                    }

                    userMemberPaginate.Message = response.Message;
                    return false;
                }

                // 如果不是全頁導出
                if (!isFullPageOut)
                {                    
                    // 獲取當前頁面的使用者資料
                    if (!await FetchUserRepresentations(userMemberSearch.First, userMemberSearch.Max))
                    {
                        return userMemberPaginate;  // 若請求失敗則返回
                    }
                }
                else
                {
                    // 全頁導出：分批次取得所有使用者資料
                    const int batchSize = 100;
                    for (int offset = 0; offset <= usersCount.Data; offset += batchSize)
                    {
                        // 獲取當前頁面的使用者資料
                        if (!await FetchUserRepresentations(offset, batchSize))
                        {
                            return userMemberPaginate;  // 若請求失敗則返回
                        }
                    }                    
                }                

                //取得成員資料
                userMemberPaginate = new()
                {
                    ViewModels = mapper.Map<List<UserMemberViewModel>>(userRepresentations),
                    PageNumber = userMemberSearch.PageNumber,
                    PageSize = userMemberSearch.PageSize,
                };

                foreach (UserMemberViewModel userMember in userMemberPaginate.ViewModels)
                {
                    ResponseModel<IList<GroupRepresentation>> groupResponse = await adminService.FindUserGroups(userMember.Id);
                    if (groupResponse is { Code: (int)KeycloakResponseCode.Success, Data: not null })
                    {
                        foreach (GroupRepresentation groupRepresentation in groupResponse.Data)
                        {
                            userMember.Groups.Add(groupRepresentation.Name ?? string.Empty);
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(userMemberSearch.UserGroupName))
                {
                    userMemberPaginate.ViewModels = userMemberPaginate.ViewModels.Where(x => x.Groups.Contains(userMemberSearch.UserGroupName)).ToList();
                }

                userMemberPaginate.TotalCount = usersCount.Data;                
                userMemberPaginate.Success();
            }
            else
            {
                userMemberPaginate.DbNoData();
            }
            return userMemberPaginate;
        }

        /// <summary>
        /// 登入日誌
        /// </summary>        
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public async Task LogLogin(UserInfo userInfo)
        {
            OperationLogSave operationLogSave = new()
            {
                ActionType = ActionType.Login,
            };

            // 呼叫現有的 SaveOperationLog 方法
            await SaveOperationLog(operationLogSave, userInfo.UserName, $"{userInfo.FirstName}{userInfo.LastName}");
        }

        /// <summary>
        /// 操作紀錄(傳到MongoDB)
        /// </summary>        
        /// <param name="operationLogSave"></param>
        /// <param name="userName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task SaveOperationLog(OperationLogSave operationLogSave, string userId = "test", string userName = "test")
        {
            await operationLog.InsertOneAsync(MapFrom<OperationLog, OperationLogSave>(operationLogSave, OperateType.Search, userId, userName));
        }

        /// <summary>
        /// 客戶印鑑事件紀錄(傳到MongoDB)
        /// </summary>        
        /// <param name="customerSealGroupLogSave"></param>
        /// <param name="operateType"></param>
        /// <param name="userName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task SaveCustomerSealEventLog(CustomerSealEventLogSave customerSealGroupLogSave, OperateType operateType, string userId = "test", string userName = "test")
        {
            await customerSealEventLog.InsertOneAsync(MapFrom<CustomerSealEventLog, CustomerSealEventLogSave>(customerSealGroupLogSave, operateType, userId, userName));
        }

        /// <summary>
        /// 會計師簽印事件紀錄(傳到MongoDB)
        /// </summary>        
        /// <param name="accountantSignEventLogSave"></param>
        /// <param name="operateType"></param>
        /// <param name="userName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task SaveAccountantSignEventLog(AccountantSignEventLogSave accountantSignEventLogSave, OperateType operateType, string userId = "test", string userName = "test")
        {
            await accountantSignEventLog.InsertOneAsync(MapFrom<AccountantSignEventLog, AccountantSignEventLogSave>(accountantSignEventLogSave, operateType, userId, userName));
        }

        private static T MapFrom<T, K>(K logSaveData, OperateType operateType, string userId, string userName) where T : LogModel<K, DateTime>, new()
        {
            if (logSaveData == null) throw new ArgumentNullException(nameof(logSaveData));

            return new()
            {
                Data = logSaveData,                
                OperateType = operateType,
                FunctionType = FunctionType.SealTypographic,
                LogLevel = CommonLib.Enums.LogLevel.Info,
                SystemType = SystemType.SealTypographic,
                UserId = userId,
                UserName = userName
            };
        }
    }
}
