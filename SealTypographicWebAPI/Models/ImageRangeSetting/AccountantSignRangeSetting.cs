using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.ImageRangeSetting
{
    /// <summary>
    /// 會計師簽印範圍設定
    /// </summary>
    public class AccountantSignRangeSetting : BaseTemplate
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 會計師簽印範圍設定
        /// </summary>
        public IList<AccountantSignRangeLocation> AccountantSignRangeLocations { get; set; }
    }
}
