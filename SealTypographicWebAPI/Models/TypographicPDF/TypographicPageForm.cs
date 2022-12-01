namespace SealTypographicWebAPI.Models.TypographicPDF
{
    /// <summary>
    /// 排版資訊
    /// </summary>
    public class TypographicPageForm
    {
        /// <summary>
        /// 頁數
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// PDFID
        /// </summary>
        public int TypographicPDFId { get; set; }

        /// <summary>
        /// 客戶印鑑位置
        /// </summary>
        public List<CustomerSealLocationViewModel> CustomerSealLocationViewModels { get; set; }

        /// <summary>
        /// 會計師簽名印鑑位置
        /// </summary>
        public List<AccountantSingLocationViewModel> AccountantSingLocationViewModels { get; set; }

        /// <summary>
        /// 信頭圖片位置
        /// </summary>
        public List<LetterheadImageLocationViewModel> LetterheadImageLocationViewModels { get; set; }
    }
}
