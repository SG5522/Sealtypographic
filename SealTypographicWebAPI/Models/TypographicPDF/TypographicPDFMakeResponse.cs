using DJLib.Models;
using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models.TypographicPDF
{
    /// <summary>
    /// 排版PDF + 回應訊息
    /// </summary>
    public class TypographicPDFMakeResponse : ResponseViewModel
    {
        /// <summary>
        /// PDF圖檔(base64)
        /// </summary>
        public string PDFBase64 { get; set; }
    }
}
