using AutoMapper;
using DBEntities;
using DBEntities.Consts;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Models.LogReport;
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
        //private readonly IAdminService adminService;

        /// <summary>
        /// 建置
        /// </summary>
        /// <param name="dbContext"></param>        
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public LogReportService (SealTypographicDbContext dbContext, ILogger<LogReportService> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;
            configurationProvider = mapper.ConfigurationProvider;
            //this.adminService = adminService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerTypoReportSearch"></param>
        /// <param name="typographyType"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public TypographicReportPaginate GetTypographicReport(TypographicReportSearch customerTypoReportSearch, TypographyType typographyType, int userId = 0)
        {
            logger.LogInformation("GetCustomerTypoReport input customerTypoReportSearch: {@customerTypoReportSearch} typographyType: {@typographyType} userId: {@userId}"
                , customerTypoReportSearch, typographyType, userId);

            TypographicReportPaginate customerTypoReportPaginate = new ();
            try
            {
                IQueryable<TypographicPDF> typographicPDFQuery = dbContext.TypographicPDFs
                                                                .Include(x => x.Customer)
                                                                .Where(
                                                                            x => x.DeleteStatus == DeleteStatus.No
                                                                            && x.TypographyType == typographyType
                                                                            && x.ReviewStatus == ReviewStatus.Approval
                                                                            && x.UpdateDate >= customerTypoReportSearch.StartDate
                                                                            && x.UpdateDate < customerTypoReportSearch.EndDate
                                                                      );

                if(!string.IsNullOrEmpty(customerTypoReportSearch.UserKeyWord))
                {
                    //TODO:這邊之後先用Keycloak 模糊搜尋找到User                    
                    List<int> userIds = new()
                    {
                        0,1,2,3,4,5
                    };
                    //adminService.GetUser
                    //TODO:或是所有的登入資料要先放到資料表user中在過濾出來。
                    //List<int> userIds = dbContext.Users.Where(x => x.UserName.Contains(customerTypoReportSearch.UserKeyWord)).Select(x => x.Id).ToList();                    
                    //adminService.FindUsers(username: customerTypoReportSearch.UserKeyWord);
                    typographicPDFQuery = typographicPDFQuery.Where(x => userIds.Contains(x.UpdateUserId));
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

                logger.LogInformation("GetCustomerTypoReport output {@customerTypoReportPaginate}", customerTypoReportPaginate);
            }
            catch (Exception ex) 
            {                
                logger.LogError("GetCustomerTypoReport error", ex.Message);
            }
            return customerTypoReportPaginate;
        }
    }
}
