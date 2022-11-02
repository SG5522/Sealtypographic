namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// 匯入AppConfig資料
    /// </summary>
    public class ScanConfigPath
    {
        /// <summary>
        /// 掃圖完的路徑
        /// </summary>
        public string ScanImagePath { get; set; } = null!;
        /// <summary>
        /// 暫存資料夾
        /// </summary>
        public string SealTempPath { get; set; } = null!;
    }
}
