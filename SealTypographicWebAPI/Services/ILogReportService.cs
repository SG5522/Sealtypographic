using CommonLib.Enums;
using DBEntities.Consts;
using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.LogReport;
using SealTypographicWebAPI.Models.LogReport.AccountantList;
using SealTypographicWebAPI.Models.LogReport.AccountantMember;
using SealTypographicWebAPI.Models.LogReport.AccountantSignLog;
using SealTypographicWebAPI.Models.LogReport.CustomerSealEventLog;
using SealTypographicWebAPI.Models.LogReport.OperationLog;
using SealTypographicWebAPI.Models.LogReport.TypographicReport;
using SealTypographicWebAPI.Models.LogReport.UserMember;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// 報表管理Service
    /// 紀錄操作(主要為查詢)、客戶印鑑異動、會計師簽印異動
    /// </summary>
    public interface ILogReportService
    {

        /// <summary>
        /// 取得動作類別名稱
        /// </summary>
        /// <returns></returns>
        ActionTypeResponse GetActionType();

        /// <summary>
        /// 取得操作紀錄分頁列表
        /// </summary>
        /// <param name="operationLogSearch">操作紀錄查詢</param>
        /// <param name="isFullPageOut">是否全頁輸出</param>
        /// <returns></returns>
        OperationLogPaginate GetOperationLogPaginate(OperationLogSearch operationLogSearch, bool isFullPageOut = false);

        /// <summary>
        /// 取得客戶印鑑異動分頁列表
        /// </summary>
        /// <param name="customerSealEventLogSearch">客戶印鑑紀錄查詢</param>
        /// <param name="typographyType">排版類別</param>
        /// <param name="isFullPageOut">是否全部輸出</param>
        /// <returns></returns>
        CustomerSealEventLogPaginate GetCustomerSealEventLogPaginate(CustomerSealEventLogSearch customerSealEventLogSearch, TypographyType typographyType, bool isFullPageOut = false);

        /// <summary>
        /// 取得會計師簽印分頁列表
        /// </summary>
        /// <param name="accountantSignEventLogSearch">會計師簽印紀錄查詢</param>
        /// <param name="isFullPageOut">是否全部輸出</param>
        /// <returns></returns>
        AccountantSignEventLogPaginate GetAccountantSignEventLogPaginate(AccountantSignEventLogSearch accountantSignEventLogSearch, bool isFullPageOut = false);

        /// <summary>
        /// 取得排版紀錄
        /// </summary>
        /// <param name="typographicReportSearch">排版紀錄查詢</param>
        /// <param name="typographyType">排版類別</param>
        /// <param name="userId">使用者Id</param>
        /// <param name="isFullPageOut">是否全部輸出</param>
        /// <returns></returns>
        TypographicReportPaginate GetTypographicReport(TypographicReportSearch typographicReportSearch, TypographyType typographyType, int userId = 1, bool isFullPageOut = false);

        /// <summary>
        /// 取得會計師成員分頁列表
        /// </summary>
        /// <param name="accountantMemberSearch">會計師成員查詢</param>
        /// <param name="userId">使用者Id</param>
        /// <param name="isFullPageOut">是否全部輸出</param>
        /// <returns></returns>
        AccountantMemberPaginate GetAccountantMemberPaginate(AccountantMemberSearch accountantMemberSearch, int userId = 1, bool isFullPageOut = false);

        /// <summary>
        /// 取得使用者資料
        /// </summary>
        /// <param name="userMemberSearch">使用者成員查詢</param>
        /// <param name="isFullPageOut">是否全部輸出</param>
        /// <returns></returns>
        Task<UserMemberPaginate> GetUserMember([FromQuery] UserMemberSearch userMemberSearch, bool isFullPageOut = false);

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
        Task SaveOperationLog(OperationLogSave operationLogForm, string userId = "test", string userName = "test");

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
