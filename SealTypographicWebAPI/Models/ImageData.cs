namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageData
    {
        /// <summary>
        /// 檔名
        /// </summary>
        public string? FileName { get; set; }
        /// <summary>
        /// 完整路徑檔名
        /// </summary>
        public string? FileFullName { get; set; }
        /// <summary>
        /// 檔案Type
        /// </summary>
        public string ContentType { get; set; } = null!;
        /// <summary>
        /// byte圖檔
        /// </summary>
        public byte[] Data { get; set; } = null!;
    }
}
