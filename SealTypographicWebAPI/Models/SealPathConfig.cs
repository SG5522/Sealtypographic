namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// 匯入AppConfig資料
    /// </summary>
    public class SealPathConfig
    {
        /// <summary>
        /// 印鑑路徑
        /// </summary>
        public string SealRootPath { get; set; } = string.Empty;

        /// <summary>
        /// 客戶印鑑放置路徑
        /// </summary>
        public string Customer { get; set; } = string.Empty;

        /// <summary>
        /// 會計師簽印放置路徑
        /// </summary>
        public string Accountant { get; set; } = string.Empty;

        /// <summary>
        /// 信頭放置路徑
        /// </summary>
        public string Letterhead { get; set; } = string.Empty;

    }
}
