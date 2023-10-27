using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.SealCaptureRange
{
    /// <summary>
    /// 客戶印鑑截取設定
    /// </summary>
    public class CustomerSealSetting : BaseTemplate
    {
        /// <summary>
        /// 客戶印鑑截取設定
        /// </summary>
        public IList<CustomerSealLocationSetting> CustomerSealLocationSettings { get; set; }
    }
}
