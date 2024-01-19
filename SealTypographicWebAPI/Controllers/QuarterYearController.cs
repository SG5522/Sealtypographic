using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models.QuarterYear;
using SealTypographicWebAPI.Services;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 取得季度、年度資料
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class QuarterYearController : ControllerBase
    {
        /// <summary>
        /// 季度、年度管理的Service
        /// </summary>
        private readonly IQuarterYearService quarterYearService;
        private readonly ILogger<QuarterYearController> logger;

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="quarterYearService"></param>
        public QuarterYearController(ILogger<QuarterYearController> logger, IQuarterYearService quarterYearService)
        {
            this.quarterYearService = quarterYearService;
            this.logger = logger;
        }

        /// <summary>
        /// 取得季度列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public QuarterResponse QuarterList()
        {
            QuarterResponse financialQuarterResponse = new();
            
            try
            {
                financialQuarterResponse = quarterYearService.GetQuarters();
                //logger.LogInformation("FinancialQuarterList output {@Output}", financialQuarterResponse);
            }
            catch (Exception ex) 
            {
                logger.LogError("FinancialQuarterList error {@Error}", ex.Message);
                financialQuarterResponse.Error();
            }
            return financialQuarterResponse;
        }

        /// <summary>
        /// 取得年度列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public YearResponse YearList()
        {
            YearResponse taxYearResponse = new();

            try
            {
                taxYearResponse = quarterYearService.GetYears();
                //logger.LogInformation("TaxYearList output {@Output}", taxYearResponse);
            }
            catch (Exception ex)
            {
                logger.LogError("TaxYearList error {@Error}", ex.Message);
                taxYearResponse.Error();
            }
            return taxYearResponse;
        }

    }
}
