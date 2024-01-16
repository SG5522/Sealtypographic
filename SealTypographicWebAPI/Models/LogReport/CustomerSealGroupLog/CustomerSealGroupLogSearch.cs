using DBEntities.Consts;

namespace SealTypographicWebAPI.Models.LogReport.SealGroupLog
{
    /// <summary>
    /// 操作紀錄
    /// </summary>
    public class CustomerSealGroupLogSearch : LogSearchBase
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
        public string? CustomerName { get; set; }
    }
}
