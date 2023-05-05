namespace SealTypographicWebAPI.Models.TypographicPDF
{
    /// <summary>
    /// 排版資訊
    /// </summary>
    public class TypographicPDFSaveForm
    {
        /// <summary>
        /// PDFID
        /// </summary>        
        public int TypographicPDFId { get; set; }


        /// <summary>
        /// 排版頁數
        /// </summary>
        public List<TypographicPageForm> TypographicPageModels { get; set;}
    }
}
