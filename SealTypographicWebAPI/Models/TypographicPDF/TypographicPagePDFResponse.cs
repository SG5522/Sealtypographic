namespace SealTypographicWebAPI.Models.TypographicPDF
{
    /// <summary>
    /// 排版PDF + 回應訊息
    /// </summary>
    public class TypographicPagePDFResponse : ResponseViewModel
    {
        /// <summary>
        /// PDFBase64
        /// </summary>
        public string PDFBase64 { get; set; }
    }
}
