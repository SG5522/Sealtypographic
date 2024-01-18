using AutoMapper;
using CommonLib.Enums;
using CommonLib.Models;
using DBEntities;
using DBEntities.Consts;
using DBEntities.Entities.TypographicModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SealTypographicWebAPI.Config;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models.LogReport.AccountantSignLog;
using SealTypographicWebAPI.Models.LogReport.CustomerSealEventLog;
using SealTypographicWebAPI.Models.LogReport.OperationLog;
using SealTypographicWebAPI.Models.LogReport.TypographicReport;
using SealTypographicWebAPI.Models.MongoDBModel;
using SealTypographicWebAPI.Utils;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 
    /// </summary>
    public class LogReportService : ILogReportService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly ILogger<LogReportService> logger;        
        private readonly AutoMapper.IConfigurationProvider configurationProvider;        
        private IMongoCollection<OperationLog> operationLog;
        private IMongoCollection<CustomerSealEventLog> customerSealEventLog;
        private IMongoCollection<AccountantSignEventLog> accountantSignEventLog;

        /// <summary>
        /// 建置
        /// </summary>
        /// <param name="dbContext"></param>        
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="options"></param>
        public LogReportService(SealTypographicDbContext dbContext, ILogger<LogReportService> logger, IMapper mapper, IOptionsMonitor<LogDatabaseOptions> options)
        {
            this.dbContext = dbContext;
            this.logger = logger;            
            configurationProvider = mapper.ConfigurationProvider;
            //MongoDb連線
            MongoClient mongoClient = new (options.CurrentValue.ConnectionString);
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
        /// 操作紀錄分頁列表
        /// </summary>
        /// <returns></returns>
        public OperationLogPaginate GetOperationLogPaginate(OperationLogSearch operationLogSearch)
        {
            OperationLogPaginate operationLogPaginate = new();

            logger.LogInformation("OperationLogPaginate input operationLogSearch: {@operationLogSearch}", operationLogSearch);

            IQueryable<OperationLog> operationLogQuery = operationLog.AsQueryable().Where
                                                        (
                                                            x => x.DateTime >= operationLogSearch.StartDate 
                                                            && x.DateTime <= operationLogSearch.EndDate                                                            
                                                        );

            if (operationLogSearch.ActionType != null)
            {
                operationLogQuery = operationLogQuery.Where(x => x.Data!.ActionType == operationLogSearch.ActionType);
            }

            if (!string.IsNullOrWhiteSpace(operationLogSearch.UserKeyWord))
            {
                operationLogQuery = operationLogQuery.Where(x => x.UserId.ToLower().Contains(operationLogSearch.UserKeyWord.ToLower())
                                                            || x.UserName.ToLower().Contains(operationLogSearch.UserKeyWord.ToLower()));
            }

            if(!string.IsNullOrWhiteSpace(operationLogSearch.TargetKeyWord))
            {
                operationLogQuery = operationLogQuery.Where(x => x.Data != null &&
                                                                (
                                                                    x.Data!.CustomerName.ToLower().Contains(operationLogSearch.TargetKeyWord.ToLower())
                                                                    || x.Data!.AccountantName.ToLower().Contains(operationLogSearch.TargetKeyWord.ToLower())
                                                                )
                                                            );                
            }

            if(operationLogQuery.Any())
            {
                operationLogPaginate.ViewModels = PageUtil.SetPaginateViewModel<OperationLog, OperationLogViewModel>
                                                    (operationLogQuery, operationLogSearch.PageNumber, operationLogSearch.PageSize, configurationProvider);

                PageUtil.SetPaginate(operationLogPaginate, operationLogSearch.PageNumber, operationLogSearch.PageSize, operationLogQuery.Count());

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
        public CustomerSealEventLogPaginate GetCustomerSealEventLogPaginate(CustomerSealEventLogSearch customerSealEventLogSearch, TypographyType typographyType)
        {
            CustomerSealEventLogPaginate customerSealEventLogPaginate = new();

            logger.LogInformation("OperationLogPaginate input operationLogSearch: {@operationLogSearch}", customerSealEventLogSearch);

            IQueryable<CustomerSealEventLog> customerSealEventLogQuery = customerSealEventLog.AsQueryable().Where
                                                        (
                                                            x => x.DateTime >= customerSealEventLogSearch.StartDate
                                                            && x.DateTime <= customerSealEventLogSearch.EndDate
                                                            && x.Data!.TypographyType == typographyType
                                                        );

            if (customerSealEventLogSearch.ReviewStatus != null)
            {
                customerSealEventLogQuery = customerSealEventLogQuery.Where(x => x.Data!.ReviewStatus == customerSealEventLogSearch.ReviewStatus);
            }

            if (!string.IsNullOrWhiteSpace(customerSealEventLogSearch.UserKeyWord))
            {
                customerSealEventLogQuery = customerSealEventLogQuery.Where(x => x.UserId.ToLower().Contains(customerSealEventLogSearch.UserKeyWord.ToLower())
                                                            || x.UserName.ToLower().Contains(customerSealEventLogSearch.UserKeyWord.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(customerSealEventLogSearch.CustomerKeyWord))
            {
                customerSealEventLogQuery = customerSealEventLogQuery.Where(x => x.Data != null &&
                                                                (
                                                                    x.Data!.CustomerCode.ToLower().Contains(customerSealEventLogSearch.CustomerKeyWord.ToLower())
                                                                    || x.Data!.CustomerName.ToLower().Contains(customerSealEventLogSearch.CustomerKeyWord.ToLower())
                                                                )
                                                            );
            }

            if (customerSealEventLogQuery.Any())
            {
                customerSealEventLogPaginate.ViewModels = PageUtil.SetPaginateViewModel<CustomerSealEventLog, CustomerSealEventLogViewModel>
                                                    (customerSealEventLogQuery, customerSealEventLogSearch.PageNumber, customerSealEventLogSearch.PageSize, configurationProvider);

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
        /// 客戶印鑑異動分頁列表
        /// </summary>
        /// <returns></returns>
        public AccountantSignEventLogPaginate GetAccountantSignEventLogPaginate(AccountantSignEventLogSearch accountantSignEventLogSearch)
        {
            AccountantSignEventLogPaginate accountantSignEventLogPaginate = new();

            logger.LogInformation("OperationLogPaginate input operationLogSearch: {@operationLogSearch}", accountantSignEventLogSearch);

            IQueryable<AccountantSignEventLog> accountantSignEventLogQuery = accountantSignEventLog.AsQueryable().Where
                                                        (
                                                            x => x.DateTime >= accountantSignEventLogSearch.StartDate
                                                            && x.DateTime <= accountantSignEventLogSearch.EndDate
                                                        );

            if (accountantSignEventLogSearch.ReviewStatus != null)
            {
                accountantSignEventLogQuery = accountantSignEventLogQuery.Where(x => x.Data!.ReviewStatus == accountantSignEventLogSearch.ReviewStatus);
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
                                                                    x.Data!.AccountantCode.ToLower().Contains(accountantSignEventLogSearch.AccountantKeyword.ToLower())
                                                                    || x.Data!.AccountantName.ToLower().Contains(accountantSignEventLogSearch.AccountantKeyword.ToLower())
                                                                )
                                                            );
            }

            if (accountantSignEventLogQuery.Any())
            {
                accountantSignEventLogPaginate.ViewModels = PageUtil.SetPaginateViewModel<AccountantSignEventLog, AccountantSignEventLogViewModel>
                                                    (accountantSignEventLogQuery, accountantSignEventLogSearch.PageNumber, accountantSignEventLogSearch.PageSize, configurationProvider);

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
        /// 
        /// </summary>
        /// <param name="customerTypoReportSearch"></param>
        /// <param name="typographyType"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public TypographicReportPaginate GetTypographicReport(TypographicReportSearch customerTypoReportSearch, TypographyType typographyType, int userId = 1)
        {
            logger.LogInformation("GetTypographicReport input customerTypoReportSearch: {@customerTypoReportSearch} typographyType: {@typographyType} userId: {@userId}"
                , customerTypoReportSearch, typographyType, userId);

            TypographicReportPaginate customerTypoReportPaginate = new ();
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

                if(!string.IsNullOrEmpty(customerTypoReportSearch.UserKeyWord))
                {
                    //adminService.GetUser
                    //TODO:或是所有的登入資料要先放到資料表user中在過濾出來。
                    typographicPDFQuery = typographicPDFQuery.Where(x => x.UpdateUser!.LastName!.Contains(customerTypoReportSearch.UserKeyWord)
                                                                    || x.UpdateUser!.UserName!.Contains(customerTypoReportSearch.UserKeyWord));
                }

                if(!string.IsNullOrEmpty(customerTypoReportSearch.CustomerKeyWord))
                {
                    typographicPDFQuery = typographicPDFQuery.Where(x => x.Customer.Name.Contains(customerTypoReportSearch.CustomerKeyWord)
                                                                    || x.Customer.Code.Contains(customerTypoReportSearch.CustomerKeyWord));                 
                }

                typographicPDFQuery = typographicPDFQuery.OrderBy(x => x.Id);

                if (typographicPDFQuery.Any())
                {

                    customerTypoReportPaginate.ViewModels = PageUtil.SetPaginateViewModel<TypographicPDF, TypographicReportViewModel>
                                                            (typographicPDFQuery, customerTypoReportSearch.PageNumber, customerTypoReportSearch.PageSize, configurationProvider);
                    

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
        /// 操作紀錄(傳到MongoDB)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="userName"></param>
        /// <param name="userId"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public void SaveCustomizeLog<T>(T data, string userName = "test", string userId = "test")
        {
            LogModel<T> logModel = new()
            {
                Data = data,
                SystemType = SystemType.SealTypographic,
                OperateType = OperateType.Search,
                FunctionType = FunctionType.SealTypographic,                                
                LogLevel = CommonLib.Enums.LogLevel.Info,
                DateTime = DateTime.Now,                          
                UserId = userId,
                UserName = userName
            };
        }

        private static T MapFrom<T, K>(K logSaveData, OperateType operateType, string userId, string userName) where T : LogModel<K>, new()             
        {
            if (logSaveData == null) throw new ArgumentNullException(nameof(logSaveData));

            return new ()
            {
                Data = logSaveData,
                DateTime = DateTime.Now,
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
