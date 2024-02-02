using DBEntities.Consts;
using DJSpire.Models;
using DJSpire.Utils;
using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models.LogReport.AccountantSignLog;
using SealTypographicWebAPI.Models.LogReport.CustomerSealEventLog;
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
        private readonly ILogger<ReportController> logger;
        private const string EXCEL_CONTENT_TYPE = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly List<string> operationLogHeaders = new()
        {
            "使用者ID",
            "使用者名稱",
            "紀錄日期",
            "動作",
            "查詢對象",
            "印鑑季度/年度"
        };

        /// <summary>
        /// 建置
        /// </summary>
        public ReportController(ILogReportService logReportService, ILogger<ReportController> logger)
        {
            this.logReportService = logReportService;
            this.logger = logger;
        }


        /// <summary>
        /// 取得動作類別名稱
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public ActionTypeResponse ActionType() => logReportService.GetActionType();

        /// <summary>
        /// 取得操作紀錄分頁列表
        /// </summary>
        /// <param name="operationLogSearch"></param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public OperationLogPaginate OperationLogPaginate ([FromQuery] OperationLogSearch operationLogSearch)
            => logReportService.GetOperationLogPaginate(operationLogSearch);

        /// <summary>
        /// 操作紀錄輸出Excel(全頁輸出)        
        /// </summary>
        /// <param name="operationLogSearch"></param>
        /// <param name="fileName">預設為OperationLog</param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public IActionResult OperationLogToExcel([FromQuery] OperationLogSearch operationLogSearch, string fileName = "OperationLog")
        {
            IActionResult result;

            List<OperationLogViewModel> paginatedData = logReportService.GetOperationLogPaginate(operationLogSearch, true).ViewModels;

            if (paginatedData != null)
            {
                ExcelData<OperationLogViewModel> excelData = new(paginatedData, operationLogHeaders);

                result = new FileContentResult(ExcelUtil.CreateFileToBytes(excelData), EXCEL_CONTENT_TYPE)
                {
                    FileDownloadName = $"{fileName}.xlsx"
                };
            }
            else
            {
                // 當沒有資料時的處理邏輯                
                result = new ObjectResult("No data found")
                {
                    StatusCode = 204 // 204 表示 No Content
                };
            }

            return result;
        }

        /// <summary>
        /// 取得財報印鑑異動紀錄分頁列表
        /// </summary>
        /// <param name="customerSealEventLogSearch"></param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public CustomerSealEventLogPaginate FinancialReportSealEventLogPaginate([FromQuery] CustomerSealEventLogSearch customerSealEventLogSearch)
            => logReportService.GetCustomerSealEventLogPaginate(customerSealEventLogSearch, TypographyType.FinancialReport);

        /// <summary>
        /// 取得稅報印鑑異動紀錄分頁列表
        /// </summary>
        /// <param name="customerSealEventLogSearch"></param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public CustomerSealEventLogPaginate TaxReportSealEventLogPaginate([FromQuery] CustomerSealEventLogSearch customerSealEventLogSearch)
            => logReportService.GetCustomerSealEventLogPaginate(customerSealEventLogSearch, TypographyType.TaxReport);

        /// <summary>
        /// 取得會計師簽印異動紀錄分頁列表
        /// </summary>
        /// <param name="accountantSignEventLogSearch"></param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public AccountantSignEventLogPaginate AccountantSignEventLogPaginate([FromQuery] AccountantSignEventLogSearch accountantSignEventLogSearch)
            => logReportService.GetAccountantSignEventLogPaginate(accountantSignEventLogSearch);

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
