using DBEntities.Base;

namespace DBEntities
{
    /// <summary>
    /// 排版頁
    /// </summary>
    public class TypographicPage : BaseData
    {

        /// <summary>
        /// 頁數
        /// </summary>
        public int PageNumber { get; set; }

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
        public List<AccountantSignLocation> AccountantSignLocaltions { get; set; }

        /// <summary>
        /// 信頭圖片排版位置
        /// </summary>
        public List<LetterheadImageLocation> LetterheadImageLocaltions { get; set; }

        /// <summary>
        /// 臨時章排版位置
        /// </summary>
        public List<TemporarySealLocation> TemporarySealJournals { get; set; }
    }
}
