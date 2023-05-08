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
        /// 排版頁面資訊
        /// </summary>
        public List<TypographicPageForm> Pages { get; set; }
    }
}
