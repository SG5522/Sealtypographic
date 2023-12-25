namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// PDF圖檔資訊
    /// </summary>
    public class PDFImageInfo
    {
        /// <summary>
        /// 圖片寬度
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 圖片高度
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Pdf總頁次
        /// </summary>
        public int TotalPage { get; set; }

        /// <summary>
        /// Imagebase64
        /// </summary>
        public string ImageBase64 { get; set; }
        
    }
}
