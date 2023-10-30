namespace SealTypographicWebAPI.Models.SealCaptureRange
{
    /// <summary>
    /// 客戶印鑑分離截取設定
    /// </summary>
    public class AccountantSignCaptureResponse : ResponseViewModel
    {
        /// <summary>
        /// 客戶印鑑截取設定
        /// </summary>
        public AccountantSignCaptureSetting AccountantSignCaptureSetting { get; set; }        
    }
}
