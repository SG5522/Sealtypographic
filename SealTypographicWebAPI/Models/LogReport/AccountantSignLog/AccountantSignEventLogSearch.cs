using DBEntities.Consts;

namespace SealTypographicWebAPI.Models.LogReport.AccountantSignLog
{
    /// <summary>
    /// 操作紀錄
    /// </summary>
    public class AccountantSignEventLogSearch : LogSearchBase
    {
        /// <summary>
        /// 動作類別(紀錄使用)
        /// </summary>
        public ReviewStatus? ReviewStatus { get; set; }

        /// <summary>
        /// UserId or UserName
        /// </summary>
        public string? KeyWord { get; set; }

        /// <summary>
        /// 查詢對象
        /// </summary>
        public string? AccountantCodeOrName { get; set; }
    }
}
