namespace SealTypographicWebAPI.Models.TypographicPDF
{
    /// <summary>
    /// 排版資訊
    /// </summary>
    public class TypographicPDFForm : ResponseViewModel
    {
        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 原始檔名
        /// </summary>
        public string OriginFileName { get; set; }

        /// <summary>
        /// PDFBase64
        /// </summary>
        public string PDFBase64 { get; set; }

        /// <summary>
        /// 季度
        /// </summary>
        public string Quarter { get; set; }

        /// <summary>
        /// 客戶名稱
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// 排版頁數
        /// </summary>
        public List<TypographicPageForm> TypographicPagesForm { get; set;}
    }
}
