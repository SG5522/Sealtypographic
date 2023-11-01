using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.ImageRangeSetting
{
    /// <summary>
    /// 客戶印鑑截取設定
    /// </summary>
    public class AccountantSignRangeSetting : BaseTemplate
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 客戶印鑑截取範圍設定
        /// </summary>
        public IList<AccountantSignRangeLocation> AccountantSignRangeLocations { get; set; }
    }
}
