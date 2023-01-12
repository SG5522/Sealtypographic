namespace SealTypographicWebAPI.Models.Upload
{
    /// <summary>
    /// 匯入AppConfig資料
    /// </summary>
    public class UploadPathConfig
    {
        /// <summary>
        /// 上傳資料根目錄放置路徑
        /// </summary>
        public string UploadRootPath { get; set; }

        /// <summary>
        /// 客戶印鑑授權書放置路徑
        /// </summary>
        public string Customer { get; set; } = string.Empty;

        /// <summary>
        /// 會計師授權書放置路徑
        /// </summary>
        public string Accountant { get; set; } = string.Empty;

        /// <summary>
        /// 信頭授權書放置路徑
        /// </summary>
        public string Letterhead { get; set; } = string.Empty;

        /// <summary>
        /// PDF放置路徑
        /// </summary>
        public string PDF { get; set; } = string.Empty;

    }
}
