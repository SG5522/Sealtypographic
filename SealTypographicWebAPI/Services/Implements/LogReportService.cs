using AutoMapper;
using DBEntities;
using SealTypographicWebAPI.Models.LogReport;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 
    /// </summary>
    public class LogReportService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly ILogger<LogReportService> logger;
        private readonly IMapper mapper;
        private readonly AutoMapper.IConfigurationProvider configurationProvider;
        
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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public CustomerTypoReportPaginate GetCustomerTypoReport (CustomerTypoReportSearch customerTypoReportSearch)
        {
            CustomerTypoReportPaginate customerTypoReportPaginate = new ();
            try
            {

            }
            catch (Exception ex) 
            {
                
            }
            return customerTypoReportPaginate;
        }
    }
}
