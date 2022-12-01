namespace SealTypographicWebAPI.Entities
{
    /// <summary>
    /// 排版頁
    /// </summary>
    public class TypographicPage
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 頁數
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// PDFID
        /// </summary>
        public int TypographicPDFId { get; set; }

        /// <summary>
        /// 排版PDF資料表
        /// </summary>
        public TypographicPDF TypographicPDF { get; set; }

        /// <summary>
        /// 客戶印鑑排版位置
        /// </summary>
        public List<CustomerSealLocation> CustomerSealLocaltions { get; set; }

        /// <summary>
        /// 會計師印鑑簽名排版位置
        /// </summary>
        public List<AccountantSingLocation> AccountantSingLocaltions { get; set; }

        /// <summary>
        /// 信頭圖片排版位置
        /// </summary>
        public List<LetterheadImageLocation> LetterheadImageLocaltions { get; set; }
    }
}
