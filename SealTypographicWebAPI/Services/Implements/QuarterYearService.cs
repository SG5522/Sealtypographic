using AutoMapper;
using AutoMapper.QueryableExtensions;
using DBEntities;
using DBEntities.Consts;
using SealTypographicWebAPI.Models.QuarterYear;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 財報季度、稅報年度管理
    /// </summary>
    public class QuarterYearService : IQuarterYearService
    {
        private readonly SealTypographicDbContext dbContext;        
        private readonly AutoMapper.IConfigurationProvider configurationProvider;

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        public QuarterYearService(SealTypographicDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;            
            configurationProvider = mapper.ConfigurationProvider;
        }

        /// <summary>
        /// 取得季度列表
        /// </summary>
        /// <returns></returns>
        public QuarterResponse GetQuarters()
        {
            QuarterResponse result = new();

            IQueryable<QuarterViewModel> quarterQuery = dbContext.QuarterYears
                                                        .Where(x => x.Type == TypographyType.FinancialReport)
                                                        .OrderByDescending(x => x.Id)                                                                        
                                                        .ProjectTo<QuarterViewModel>(configurationProvider);

            if(quarterQuery.Any())
            {
                result.Quarters = quarterQuery.ToList();
                result.Success();
            }
            else
            {
                result.DbNoData();
            }

            return result;
        }

        /// <summary>
        /// 取得年度列表
        /// </summary>
        /// <returns></returns>
        public YearResponse GetYears()
        {
            YearResponse result = new();

            IQueryable<YearViewModel> yearQuery = dbContext.QuarterYears
                                                .Where(x => x.Type == TypographyType.TaxReport)
                                                .OrderByDescending(x => x.Id)                                                                
                                                .ProjectTo<YearViewModel>(configurationProvider);

            if (yearQuery.Any())
            {
                result.Years = yearQuery.ToList();
                result.Success();
            }
            else
            {
                result.DbNoData();
            }

            return result;
        }
    }
}
