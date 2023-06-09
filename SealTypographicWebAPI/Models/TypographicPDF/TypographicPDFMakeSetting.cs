using Microsoft.AspNetCore.Routing.Constraints;
using Spire.Pdf.Graphics;

namespace SealTypographicWebAPI.Models.TypographicPDF
{
    /// <summary>
    /// 輸出PDF檔案時的設定
    /// </summary>
    public class TypographicPDFMakeSetting
    {
        /// <summary>
        /// PDFID
        /// </summary>
        public int TypographicPDFId {get; set; }

        /// <summary>
        /// PDF輸出顏色
        /// </summary>
        public PdfColorSpace PdfColorSpace { get; set; }

        /// <summary>
        /// 是否輸出空白頁
        /// </summary>
        public bool IsBlank { get; set; }

        /// <summary>
        /// 輸出檔名
        /// </summary>
        public string FileName { get; set; }
    }
}
