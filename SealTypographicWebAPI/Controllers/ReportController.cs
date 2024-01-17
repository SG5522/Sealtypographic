using DBEntities.Consts;
using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models.LogReport.OperationLog;
using SealTypographicWebAPI.Models.LogReport.TypographicReport;
using SealTypographicWebAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    ///  各項報表管理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ILogReportService logReportService;

        /// <summary>
        /// 建置
        /// </summary>
        public ReportController(ILogReportService logReportService) 
        {
            this.logReportService = logReportService;
        }


        /// <summary>
        /// 取得操作紀錄
        /// </summary>
        /// <param name="operationLogSearch"></param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public OperationLogPaginate OperationLog([FromQuery] OperationLogSearch operationLogSearch)
            => logReportService.OperationLogPaginate(operationLogSearch);

        /// <summary>
        /// 取得財報排版紀錄
        /// </summary>
        /// <param name="customerTypoReportSearch"></param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public TypographicReportPaginate FinancialReport([FromQuery] TypographicReportSearch customerTypoReportSearch)
            => logReportService.GetTypographicReport(customerTypoReportSearch, TypographyType.FinancialReport);

        /// <summary>
        /// 取得稅報排版紀錄
        /// </summary>
        /// <param name="customerTypoReportSearch"></param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public TypographicReportPaginate TaxReport([FromQuery] TypographicReportSearch customerTypoReportSearch)
            => logReportService.GetTypographicReport(customerTypoReportSearch, TypographyType.TaxReport);
    }
}
