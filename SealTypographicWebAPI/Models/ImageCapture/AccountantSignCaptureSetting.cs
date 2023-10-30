using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.SealCaptureRange
{
    /// <summary>
    /// 客戶印鑑截取設定
    /// </summary>
    public class AccountantSignCaptureSetting : BaseTemplate
    {
        /// <summary>
        /// 客戶印鑑截取範圍設定
        /// </summary>
        public IList<AccountantSignCaptureLocation> AccountantSignCaptureLocations { get; set; }
    }
}
