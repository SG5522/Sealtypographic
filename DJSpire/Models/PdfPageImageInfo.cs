
namespace DJSpire.Models
{
    /// <summary>
    /// PDF圖檔資訊
    /// </summary>
    public class PdfPageImageInfo
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
        /// PDF頁次
        /// </summary>
        public int PageNo { get; set; }

        /// <summary>
        /// Pdf總頁次
        /// </summary>
        public int TotalPage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ImageDataUrl { get; set; }

    }
}
