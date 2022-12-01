namespace SealTypographicWebAPI.Models.TypographicPDF
{
    /// <summary>
    /// 排版資訊
    /// </summary>
    public class TypographicPDFViewModel : ResponseViewModel
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
        /// PDF檔
        /// </summary>
        public string PDFBase64 { get; set; }

        /// <summary>
        /// 季度
        /// </summary>
        public string Quarter { get; set; }

        /// <summary>
        /// 客戶名稱
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 排版頁數
        /// </summary>
        public List<TypographicPageForm> TypographicPageForms { get; set;}
    }
}
