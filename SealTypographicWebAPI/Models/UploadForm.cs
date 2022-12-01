namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// 上傳用Class
    /// </summary>
    public class UploadForm
    {
        /// <summary>
        /// 檔名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Base64圖檔
        /// </summary>
        public string ImageBase64 { get; set; }

        /// <summary>
        /// 上傳類別
        /// 1.客戶印鑑授權書
        /// 2.會計印鑑簽名授權書
        /// 3.信頭
        /// </summary>
        public int UploadType { get; set; }
    }
}
