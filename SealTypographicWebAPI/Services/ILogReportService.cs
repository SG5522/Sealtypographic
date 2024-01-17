using DBEntities.Consts;
using SealTypographicWebAPI.Models.LogReport.CustomerSealEventLog;
using SealTypographicWebAPI.Models.LogReport.OperationLog;
using SealTypographicWebAPI.Models.LogReport.TypographicReport;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILogReportService
    {

        /// <summary>
        /// 取得排版紀錄
        /// </summary>
        /// <param name="customerTypoReportSearch"></param>
        /// <param name="typographyType"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        TypographicReportPaginate GetTypographicReport(TypographicReportSearch customerTypoReportSearch, TypographyType typographyType, int userId = 1);

        /// <summary>
        /// 操作紀錄(傳到MongoDB)
        /// </summary>
        /// <param name="operationLogForm"></param>
        /// <param name="userName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task SaveOperationLog(OperationLogSave operationLogForm, string userName = "test", string userId = "test");

        /// <summary>
        /// 客戶印鑑事件紀錄(傳到MongoDB)
        /// </summary>        
        /// <param name="customerSealGroupLogSave"></param>
        /// <param name="userName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task SaveCustomerSealEventLog(CustomerSealEventLogSave customerSealGroupLogSave, string userId = "test", string userName = "test");

        /// <summary>
        /// 取得操作紀錄
        /// </summary>
        /// <param name="operationLogSearch"></param>
        /// <returns></returns>
        OperationLogPaginate OperationLogPaginate(OperationLogSearch operationLogSearch);
    }
}
