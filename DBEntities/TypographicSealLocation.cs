using DBEntities.Base;

namespace DBEntities
{

    /// <summary>
    /// 各印鑑簽印排版位置
    /// </summary>
    public class TypographicSealLocation : BasePageLocation
    {
        /// <summary>
        /// 客戶印鑑歷程
        /// </summary>
        public CustomerSealJournal CustomerSealJournal { get; set; }

        /// <summary>
        /// 會計師印鑑簽名歷程
        /// </summary>
        public AccountantSignJournal AccountantSignJournal { get; set; }

        /// <summary>
        /// 信頭圖片歷程
        /// </summary>
        public LetterheadImageJournal LetterheadImageJournal { get; set; }

        /// <summary>
        /// 臨時章歷程
        /// </summary>
        public TemporarySealJournal TemporarySealJournal { get; set; }
    }
}
