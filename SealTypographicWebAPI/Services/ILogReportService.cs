using DBEntities.Consts;
using SealTypographicWebAPI.Models.LogReport.OperationLog;
using SealTypographicWebAPI.Models.LogReport.SealGroupLog;
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
        /// 儲存操作紀錄
        /// </summary>
        /// <param name="operationLogForm"></param>
        /// <param name="userName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task SaveOperationLog(OperationLogSave operationLogForm, string userName = "test", string userId = "test");


        /// <summary>
        /// 取得操作紀錄
        /// </summary>
        /// <param name="operationLogSearch"></param>
        /// <returns></returns>
        OperationLogPaginate OperationLogPaginate(OperationLogSearch operationLogSearch);
    }
}
