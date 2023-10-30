namespace SealTypographicWebAPI.Models.SealCaptureRange
{
    /// <summary>
    /// 客戶印鑑分離截取設定
    /// </summary>
    public class CustomerSealCaptureResponse : ResponseViewModel
    {
        /// <summary>
        /// 客戶印鑑截取設定
        /// </summary>
        public CustomerSealCaptureSetting CustomerSealCaptureSetting { get; set; }        
    }
}
