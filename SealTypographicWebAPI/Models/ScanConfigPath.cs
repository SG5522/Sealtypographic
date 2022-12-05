namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// 匯入AppConfig資料
    /// </summary>
    public class ScanConfigPath
    {
        /// <summary>
        /// 掃圖完的放置路徑
        /// </summary>
        public string ScanImagePath { get; set; } = string.Empty;

        /// <summary>
        /// 掃圖完的放置路徑
        /// </summary>
        public string CustomerFolder { get; set; } = string.Empty;

        /// <summary>
        /// 掃圖完的放置路徑
        /// </summary>
        public string AccountantFolder { get; set; } = string.Empty;

        /// <summary>
        /// 掃圖完的放置路徑
        /// </summary>
        public string LetterheadFolder { get; set; } = string.Empty;

        /// <summary>
        /// 暫存資料夾
        /// </summary>
        public string SealTempPath { get; set; } = string.Empty;
    }
}
