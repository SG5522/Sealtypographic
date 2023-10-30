using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.SealCaptureRange
{
    /// <summary>
    /// 客戶印鑑截取設定
    /// </summary>
    public class CustomerSealCaptureSetting : BaseTemplate
    {
        /// <summary>
        /// 客戶印鑑截取範圍設定
        /// </summary>
        public IList<CustomerSealCaptureLocation> CustomerSealCaptureLocations { get; set; }
    }
}
