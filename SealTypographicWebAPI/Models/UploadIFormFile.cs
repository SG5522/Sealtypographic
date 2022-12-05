namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// 上傳 (IFromFile)
    /// </summary>
    public class UploadIFormFile
    {
        /// <summary>
        /// 檔名
        /// </summary>
        public string ClientFileName { get; set; }

        /// <summary>
        /// IFormFile取得檔案
        /// </summary>
        public IFormFile FormFile { get; set; }

        /// <summary>
        /// 上傳類別
        /// 1.客戶印鑑授權書
        /// 2.會計印鑑簽名授權書
        /// 3.信頭
        /// </summary>
        public int UploadType { get; set; }
    }
}
