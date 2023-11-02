using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.ImageRangeSetting
{
    /// <summary>
    /// 客戶印鑑範圍設定
    /// </summary>
    public class CustomerSealRangeSetting : BaseTemplate
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 客戶印鑑範圍設定
        /// </summary>
        public IList<CustomerSealRangeLocation> CustomerSealRangeLocations { get; set; }
    }
}
