using DBEntities.Consts;
using DJSpire.Models;
using DJSpire.Utils;
using Microsoft.AspNetCore.Mvc;
using OpenCvSharp;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models.LogReport.AccountantList;
using SealTypographicWebAPI.Models.LogReport.AccountantMember;
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
        private const string EXCEL_CONTENT_TYPE = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        

        /// <summary>
        /// 建置
        /// </summary>
        public ReportController(ILogReportService logReportService)
        {
            this.logReportService = logReportService;
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
        /// <param name="fileName">預設檔名為OperationLog</param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public IActionResult OperationLogToExcel([FromQuery] OperationLogSearch operationLogSearch, string fileName = "OperationLog")
            => ToExcel(
                        logReportService.GetOperationLogPaginate(operationLogSearch, true).ViewModels,
                        ExcelHearderConsts.OperationLogHeaders,
                        fileName
                     );

        /// <summary>
        /// 取得財報印鑑異動紀錄分頁列表
        /// </summary>
        /// <param name="customerSealEventLogSearch"></param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public CustomerSealEventLogPaginate FinancialReportSealEventLogPaginate([FromQuery] CustomerSealEventLogSearch customerSealEventLogSearch)
            => logReportService.GetCustomerSealEventLogPaginate(customerSealEventLogSearch, TypographyType.FinancialReport);

        /// <summary>
        /// 財報印鑑異動紀錄輸出Excel(全頁輸出)        
        /// </summary>
        /// <param name="customerSealEventLogSearch"></param>        
        /// <param name="fileName">預設檔名為FinancialReportSealEventLog</param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public IActionResult FinancialReportSealEventLogToExcel([FromQuery] CustomerSealEventLogSearch customerSealEventLogSearch, string fileName = "FinancialReportSealEventLog")
                => ToExcel(
                            logReportService.GetCustomerSealEventLogPaginate(customerSealEventLogSearch, TypographyType.FinancialReport, true).ViewModels,
                            ExcelHearderConsts.FinancialReportSealEventLogHeaders,
                            fileName
                        );

        /// <summary>
        /// 取得稅報印鑑異動紀錄分頁列表
        /// </summary>
        /// <param name="customerSealEventLogSearch"></param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public CustomerSealEventLogPaginate TaxReportSealEventLogPaginate([FromQuery] CustomerSealEventLogSearch customerSealEventLogSearch)
            => logReportService.GetCustomerSealEventLogPaginate(customerSealEventLogSearch, TypographyType.TaxReport);

        /// <summary>
        /// 稅報印鑑異動紀錄輸出Excel(全頁輸出)        
        /// </summary>
        /// <param name="customerSealEventLogSearch"></param>        
        /// <param name="fileName">預設檔名為TaxReportSealEventLog</param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public IActionResult TaxReportSealEventLogToExcel([FromQuery] CustomerSealEventLogSearch customerSealEventLogSearch, string fileName = "TaxReportSealEventLog")
                => ToExcel(
                            logReportService.GetCustomerSealEventLogPaginate(customerSealEventLogSearch, TypographyType.FinancialReport, true).ViewModels,
                            ExcelHearderConsts.TaxReportSealEventLogHeaders,
                            fileName
                        );

        /// <summary>
        /// 取得會計師簽印異動紀錄分頁列表
        /// </summary>
        /// <param name="accountantSignEventLogSearch">會計師異動紀錄查詢</param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public AccountantSignEventLogPaginate AccountantSignEventLogPaginate([FromQuery] AccountantSignEventLogSearch accountantSignEventLogSearch)
            => logReportService.GetAccountantSignEventLogPaginate(accountantSignEventLogSearch);

        /// <summary>
        /// 會計師簽印異動紀錄輸出Excel(全頁輸出)        
        /// </summary>
        /// <param name="accountantSignEventLogSearch">會計師異動紀錄查詢</param>        
        /// <param name="fileName">預設為AccountantSignEventLog</param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public IActionResult AccountantSignEventLogToExcel([FromQuery] AccountantSignEventLogSearch accountantSignEventLogSearch, string fileName = "AccountantSignEventLog")
                => ToExcel(
                            logReportService.GetAccountantSignEventLogPaginate(accountantSignEventLogSearch, true).ViewModels,
                            ExcelHearderConsts.AccountantSignEventLogHeaders,
                            fileName
                        );

        /// <summary>
        /// 取得財報排版紀錄
        /// </summary>
        /// <param name="typographicReportSearch">排版紀錄查詢</param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public TypographicReportPaginate FinancialReport([FromQuery] TypographicReportSearch typographicReportSearch)
            => logReportService.GetTypographicReport(typographicReportSearch, TypographyType.FinancialReport);

        /// <summary>
        /// 取得財報排版紀錄
        /// </summary>
        /// <param name="typographicReportSearch"></param>
        /// <param name="fileName">預設FinancialReport</param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public IActionResult FinancialReportToExcel([FromQuery] TypographicReportSearch typographicReportSearch, string fileName = "FinancialReport")
                => ToExcel(
                            logReportService.GetTypographicReport(typographicReportSearch, TypographyType.FinancialReport, 1, true).ViewModels,
                            ExcelHearderConsts.FinancialTypographicLogHeaders,
                            fileName
                        );

        /// <summary>
        /// 取得稅報排版紀錄
        /// </summary>
        /// <param name="typographicReportSearch">排版紀錄查詢</param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public TypographicReportPaginate TaxReport([FromQuery] TypographicReportSearch typographicReportSearch)
            => logReportService.GetTypographicReport(typographicReportSearch, TypographyType.TaxReport);

        /// <summary>
        /// 取得稅報排版紀錄
        /// </summary>
        /// <param name="typographicReportSearch">排版紀錄查詢</param>
        /// <param name="fileName">預設檔名為TaxReport</param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public IActionResult TaxReportToExcel([FromQuery] TypographicReportSearch typographicReportSearch, string fileName = "TaxReport")
                => ToExcel(
                            logReportService.GetTypographicReport(typographicReportSearch, TypographyType.TaxReport, 1, true).ViewModels,
                            ExcelHearderConsts.TaxTypographicLogHeaders,
                            fileName
                        );

        /// <summary>
        /// 取得稅報排版紀錄
        /// </summary>
        /// <param name="accountantMemberSearch">會計師成員查詢</param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public AccountantMemberPaginate AccountantMember ([FromQuery] AccountantMemberSearch accountantMemberSearch)
            => logReportService.GetAccountantMemberPaginate(accountantMemberSearch);

        /// <summary>
        /// 取得稅報排版紀錄
        /// </summary>
        /// <param name="accountantMemberSearch">會計師成員查詢</param>        
        /// <param name="fileName">預設檔名為AccountantMember</param>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public IActionResult AccountantMemberToExcel([FromQuery] AccountantMemberSearch accountantMemberSearch, string fileName = "AccountantMember")
                => ToExcel(
                            logReportService.GetAccountantMemberPaginate(accountantMemberSearch, 1, true).ViewModels,
                            ExcelHearderConsts.AccountantHeaders,
                            fileName
                        );

        private static IActionResult ToExcel<T>(List<T> paginatedData, List<string> headers, string fileName) where T : class
        {
            IActionResult result;

            if (paginatedData != null)
            {
                ExcelData<T> excelData = new(paginatedData, headers);

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
    }
}
