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
        /// 確認是否需要插入空白頁
        /// </summary>
        public bool BlankCheck { get; set; }

        /// <summary>
        /// 確認是否為刪除頁
        /// </summary>
        public bool DeleteCheck { get; set; }

        /// <summary>
        /// 此頁是否為會計師證明書
        /// </summary>
        public bool IsAccountantCertificate { get; set; }

        /// <summary>
        /// 客戶印鑑位置
        /// </summary>        
        public List<CustomerSealLocationForm> CustomerSealLocationForms { get; set; }

        /// <summary>
        /// 會計師簽名印鑑位置
        /// </summary>
        public List<AccountantSingLocationForm> AccountantSingLocationViewForms { get; set; }

        /// <summary>
        /// 信頭圖片位置
        /// </summary>
        public List<LetterheadImageLocationForm> LetterheadImageLocationViewForms { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<TemporarySealLocationForm> TemporarySealLocationViewForms { get; set;}
    }
}
