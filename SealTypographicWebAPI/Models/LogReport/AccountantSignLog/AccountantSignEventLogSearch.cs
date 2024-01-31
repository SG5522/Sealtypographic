using DBEntities.Consts;

namespace SealTypographicWebAPI.Models.LogReport.AccountantSignLog
{
    /// <summary>
    /// 會計師異動紀錄查詢
    /// </summary>
    public class AccountantSignEventLogSearch : LogSearchBase
    {
        /// <summary>
        /// 審核狀態
        /// </summary>
        public ReviewStatus? ReviewStatus { get; set; }

        /// <summary>
        /// UserId or UserName
        /// </summary>
        public string? UserKeyword { get; set; }

        /// <summary>
        /// 查詢對象
        /// </summary>
        public string? AccountantKeyword { get; set; }
    }
}
