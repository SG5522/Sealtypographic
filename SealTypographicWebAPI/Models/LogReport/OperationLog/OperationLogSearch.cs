using SealTypographicWebAPI.Consts;

namespace SealTypographicWebAPI.Models.LogReport.OperationLog
{
    /// <summary>
    /// 操作紀錄查詢
    /// </summary>
    public class OperationLogSearch : LogSearchBase
    {
        /// <summary>
        /// 動作類別(紀錄使用)
        /// </summary>
        public ActionType? ActionType { get; set; }

        /// <summary>
        /// UserId or UserName
        /// </summary>
        public string? UserKeyWord { get; set; }

        /// <summary>
        /// 對象名稱
        /// </summary>
        public string? TargetKeyWord { get; set; }
    }
}
