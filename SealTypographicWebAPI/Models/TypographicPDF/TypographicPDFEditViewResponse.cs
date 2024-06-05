namespace SealTypographicWebAPI.Models.TypographicPDF
{
    /// <summary>
    /// 回傳排版後PDFBase64
    /// </summary>
    public class TypographicPDFEditViewResponse : ResponseViewModel
    {
        /// <summary>
        /// PDF圖檔(base64)
        /// </summary>
        public string PDFBase64 {  get; set; }
    }
}
