using SealTypographicWebAPI.Consts;

namespace SealTypographicWebAPI.Models.LogReport
{
    /// <summary>
    /// 操作紀錄
    /// </summary>
    public class OperationLogSearch : LogSearchBase
    {
        /// <summary>
        /// 動作類別(紀錄使用)
        /// </summary>
        public ActionType? ActionType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? KeyWord { get; set; }

        /// <summary>
        /// 查詢對象
        /// </summary>
        public string? ObjectName { get; set; }
    }
}
