using AutoMapper;
using CommonLib.Enums;
using CommonLib.Models;
using DBEntities;
using DBEntities.Consts;
using DBEntities.Entities.TypographicModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SealTypographicWebAPI.Config;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.LogReport;
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
        private readonly IMapper mapper;
        private readonly AutoMapper.IConfigurationProvider configurationProvider;        
        private readonly IMongoCollection<OperationLog> operationLog;
        //private readonly IAdminService adminService;

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
            this.mapper = mapper;
            configurationProvider = mapper.ConfigurationProvider;
            MongoClient mongoClient = new (options.CurrentValue.ConnectionString);
            IMongoDatabase mongoDatabase = mongoClient.GetDatabase(options.CurrentValue.DatabaseName);
            operationLog = mongoDatabase.GetCollection<OperationLog>(LogDataBaseCollectionConsts.OperationLog);
            //this.adminService = adminService;
        }

        /// <summary>
        /// 操作紀錄(傳到MongoDB)
        /// </summary>        
        /// <param name="operationLogForm"></param>
        /// <param name="userName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task SaveOperationLog(OperationLogForm operationLogForm, string userName = "test", string userId = "test")
        {
            LogModel<OperationLogForm> logModel = new()
            {
                Data = operationLogForm,
                DateTime = DateTime.Now,
                OperateType = OperateType.Search,
                FunctionType = FunctionType.SealTypographic,                     
                LogLevel = CommonLib.Enums.LogLevel.Info,
                SystemType = SystemType.SealTypographic,
                UserId = userId,
                UserName = userName,
            };            
            await operationLog.InsertOneAsync(OperationLog.MapFrom(logModel));            
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
                                                                            && x.UpdateDate < customerTypoReportSearch.EndDate
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
                    List<TypographicReportViewModel> typographicReportViewModels = PageUtil.SetPaginateViewModel<TypographicPDF, TypographicReportViewModel>
                                                                                    (typographicPDFQuery, customerTypoReportSearch.PageNumber, customerTypoReportSearch.PageSize, configurationProvider);


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
        /// <param name="log"></param>
        /// <returns></returns>
        public void SaveCustomizeLog<T>(T data, LogModel log)
        {
            LogModel<T> logModel = new()
            {
                Data = data,
                SystemType = SystemType.SealTypographic,
                OperateType = log.OperateType,
                FunctionType = FunctionType.SealTypographic,                                
                LogLevel = log.LogLevel,
                DateTime = log.DateTime,                          
                UserId = log.UserId,
                UserName = log.UserName
            };
        }
    }
}
