namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// 上傳用Class
    /// </summary>
    public class ImageUpload
    {
        /// <summary>
        /// 檔名
        /// </summary>
        public string FileName { get; set; } = null!;
        /// <summary>
        /// Base64圖檔
        /// </summary>
        public string ImageBase64 { get; set; } = null!;
    }
}
