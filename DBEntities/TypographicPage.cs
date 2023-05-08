using DBEntities.Base;
using DBEntities.Consts;

namespace DBEntities
{
    /// <summary>
    /// 排版頁
    /// </summary>
    public class TypographicPage
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

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
        /// 排版PDF資料表
        /// </summary>
        public TypographicPDF TypographicPDF { get; set; }

        /// <summary>
        /// 客戶印鑑排版位置
        /// </summary>
        public List<CustomerSealLocation> CustomerSealLocations { get; set; }

        /// <summary>
        /// 會計師印鑑簽名排版位置
        /// </summary>
        public List<AccountantSignLocation> AccountantSignLocations { get; set; }

        /// <summary>
        /// 信頭圖片排版位置
        /// </summary>
        public List<LetterheadImageLocation> LetterheadImageLocations { get; set; }

        /// <summary>
        /// 臨時章排版位置
        /// </summary>
        public List<TemporarySealLocation> TemporarySealLocations { get; set; }
    }
}
