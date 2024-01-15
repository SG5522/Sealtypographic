using DBEntities.Consts;

namespace SealTypographicWebAPI.Models.LogReport.SealGroupLog
{
    /// <summary>
    /// 印鑑異動紀錄
    /// </summary>
    public class SealGroupLogSave
    {
        /// <summary>
        /// 審核狀態
        /// </summary>
        public ReviewStatus ReviewStatus { get; set; }

        /// <summary>
        /// 客戶Id
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// 客戶名稱
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 客戶印鑑群組Id
        /// </summary>
        public int CustomerSealGroupId { get; set; }

        /// <summary>
        /// 公曆用的季度字串
        /// </summary>
        public string GregorainQuarter { get; set; }
    }
}
