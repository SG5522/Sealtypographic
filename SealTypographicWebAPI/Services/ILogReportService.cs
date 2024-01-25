using CommonLib.Enums;
using DBEntities.Consts;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.LogReport.AccountantSignLog;
using SealTypographicWebAPI.Models.LogReport.CustomerSealEventLog;
using SealTypographicWebAPI.Models.LogReport.OperationLog;
using SealTypographicWebAPI.Models.LogReport.TypographicReport;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// 報表管理Service
    /// 紀錄操作(主要為查詢)、客戶印鑑異動、會計師簽印異動
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
        /// 取得操作紀錄分頁列表
        /// </summary>
        /// <param name="operationLogSearch">操作紀錄查詢</param>
        /// <returns></returns>
        OperationLogPaginate GetOperationLogPaginate(OperationLogSearch operationLogSearch);

        /// <summary>
        /// 取得客戶印鑑異動分頁列表
        /// </summary>
        /// <param name="customerSealEventLogSearch">客戶印鑑紀錄查詢</param>
        /// <param name="typographyType">排版類別</param>
        /// <returns></returns>
        CustomerSealEventLogPaginate GetCustomerSealEventLogPaginate(CustomerSealEventLogSearch customerSealEventLogSearch, TypographyType typographyType);

        /// <summary>
        /// 取得會計師簽印分頁列表
        /// </summary>
        /// <param name="accountantSignEventLogSearch"></param>
        /// <returns></returns>
        AccountantSignEventLogPaginate GetAccountantSignEventLogPaginate(AccountantSignEventLogSearch accountantSignEventLogSearch);

        /// <summary>
        /// 登入日誌
        /// </summary>
        /// <param name="userInfo"></param>                
        /// <returns></returns>
        Task LogLogin(UserInfo userInfo);

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
        /// <param name="customerSealGroupLogSave">印鑑異動紀錄</param>
        /// <param name="operateType">操作型態</param>
        /// <param name="userName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task SaveCustomerSealEventLog(CustomerSealEventLogSave customerSealGroupLogSave, OperateType operateType, string userId = "test", string userName = "test");

        /// <summary>
        /// 會計師簽印事件紀錄(傳到MongoDB)
        /// </summary>        
        /// <param name="accountantSignEventLogSave"></param>
        /// <param name="operateType"></param>
        /// <param name="userName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task SaveAccountantSignEventLog(AccountantSignEventLogSave accountantSignEventLogSave, OperateType operateType, string userId = "test", string userName = "test");
    }
}
