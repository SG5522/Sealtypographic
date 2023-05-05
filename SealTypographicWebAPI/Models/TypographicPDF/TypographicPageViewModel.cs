namespace SealTypographicWebAPI.Models.TypographicPDF
{
    /// <summary>
    /// 排版資訊
    /// </summary>
    public class TypographicPageViewModel
    {
        /// <summary>
        /// PDFID
        /// </summary>        
        public int PDFId { get; set; }

        /// <summary>
        /// 頁數
        /// </summary>        
        public int PageNumber { get; set; }

        /// <summary>
        /// 客戶印鑑位置
        /// </summary>        
        public List<CustomerSealLocationViewModel> CustomerSealLocationForms { get; set; }

        /// <summary>
        /// 會計師簽名印鑑位置
        /// </summary>
        public List<AccountantSingLocationViewModel> AccountantSingLocationViewForms { get; set; }

        /// <summary>
        /// 信頭圖片位置
        /// </summary>
        public List<LetterheadImageLocationViewModel> LetterheadImageLocationViewForms { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<TemporarySealLocationViewModel> TemporarySealLocationViewForms { get; set;}
    }
}
