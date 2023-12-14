using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models.TypographicPDF
{
    /// <summary>
    /// 顯示PDF內容
    /// </summary>
    public class PDFViewModel : ResponseViewModel
    {
        /// <summary>
        /// PDF總頁數
        /// </summary>
        public int TotalPage { get; set; }

        /// <summary>
        /// 單頁圖片寬度
        /// </summary>
        public int ImageWidth { get; set; }

        /// <summary>
        /// 單頁圖片高度
        /// </summary>
        public int ImageHeight { get; set; }

        /// <summary>
        /// Image64(單頁顯示)
        /// </summary>
        public string ImageBase64 { get; set; }

        /// <summary>
        /// PDF路徑
        /// </summary>
        [JsonIgnore]
        public string PDFFullPath { get; set; }
    }
}
