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
        /// 上傳檔案(PDF)的ID
        /// </summary>
        public int UploadId { get; set; }

        /// <summary>
        /// 客戶Id
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// 排版頁數
        /// </summary>
        public List<TypographicPageForm> Pages { get; set;}
    }
}
