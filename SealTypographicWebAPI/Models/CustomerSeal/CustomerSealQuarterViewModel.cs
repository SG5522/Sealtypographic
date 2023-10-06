using DBEntities.Consts;

namespace SealTypographicWebAPI.Models.CustomerSeal
{
    /// <summary>
    /// 客戶印鑑季度
    /// </summary>
    public class CustomerSealQuarterViewModel
    {
        /// <summary>
        /// 印鑑季度Id
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// 印鑑季度
        /// </summary>
        /// <example>111Q1</example>
        public string? Quarter { get; set; }

        /// <summary>
        /// 印鑑審查狀態
        /// </summary>
        public ReviewStatus ReviewStatus { get; set; }
    }
}
