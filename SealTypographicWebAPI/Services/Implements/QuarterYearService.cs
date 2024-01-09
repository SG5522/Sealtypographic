using AutoMapper;
using AutoMapper.QueryableExtensions;
using DBEntities;
using DBEntities.Consts;
using DBEntities.Entities;
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
            ReNewQuarterYear();
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
            ReNewQuarterYear();
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
        
        private void ReNewQuarterYear()
        {
            int year = DateTime.Now.Year;
            IQueryable<QuarterYear> quarterYears = dbContext.QuarterYears.Where(x => x.GregorianYear == year);

            if(!quarterYears.Any())
            {
                List<QuarterYear> quarters = new();
                //(財報季度列表)
                for (int period = 1; period <= 4; period++)
                {
                    QuarterYear quarter = new()
                    {
                        GregorianYear = year,
                        Period = $"Q{period}",
                        Type = TypographyType.FinancialReport
                    };
                    quarters.Add(quarter);
                }

                QuarterYear quarterYear = new()
                {
                    GregorianYear = year,
                    Type = TypographyType.TaxReport
                };
                quarters.Add(quarterYear);

                dbContext.QuarterYears.AddRange(quarters);
                dbContext.SaveChanges();
            }
        }
    }
}
