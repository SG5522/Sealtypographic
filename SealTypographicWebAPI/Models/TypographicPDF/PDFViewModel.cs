using DBEntities.Base;
using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models.TypographicPDF
{
    /// <summary>
    /// 顯示PDF內容
    /// </summary>
    public class PDFViewModel : ResponseViewModel
    {
        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPage { get; set; }

        /// <summary>
        /// 單頁顯示
        /// </summary>
        public string PDFBase64 { get; set; }

        /// <summary>
        /// PDF路徑
        /// </summary>
        [JsonIgnore]
        public string PDFFullPath { get; set; }
    }
}
