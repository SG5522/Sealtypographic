namespace SealTypographicWebAPI.Config
{
    /// <summary>
    /// 匯入AppConfig資料
    /// </summary>
    public class UploadPathOption
    {
        /// <summary>
        /// 上傳資料根目錄放置路徑
        /// </summary>
        public string UploadRootPath { get; set; }

        /// <summary>
        /// 客戶印鑑授權書放置路徑
        /// </summary>
        public string CustomerSealAuthorization { get; set; } = string.Empty;

        /// <summary>
        /// 會計師授權書放置路徑
        /// </summary>
        public string AccountantSignAuthorization { get; set; } = string.Empty;

        /// <summary>
        /// 信頭授權書放置路徑
        /// </summary>
        public string LetterheadImage { get; set; } = string.Empty;

        /// <summary>
        /// PDF放置路徑
        /// </summary>
        public string PDF { get; set; } = string.Empty;

        /// <summary>
        /// 會計師證明書放置路徑
        /// </summary>
        public string AccountantSignCertificate { get; set; } = string.Empty;

        /// <summary>
        /// 臨時檔放置路徑
        /// </summary>
        public string Temporary { get; set; } = string.Empty;

    }
}
