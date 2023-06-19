namespace SealTypographicWebAPI.Models.TypographicPDF
{
    /// <summary>
    /// 取得所有排板頁面資訊
    /// </summary>
    public class TypographicPagesResponse : ResponseViewModel
    {
        /// <summary>
        /// new Pages
        /// </summary>
        public TypographicPagesResponse()
        {
            Pages = new();
        }

        /// <summary>
        /// TypographicPDFID
        /// </summary>        
        public int Id { get; set; }

        /// <summary>
        /// 上傳檔案的ID
        /// </summary>
        public int UploadId { get; set; }

        /// <summary>
        /// 客戶Id
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// 排版頁面資訊
        /// </summary>
        public List<TypographicPageForm> Pages { get; set; }
    }
}
