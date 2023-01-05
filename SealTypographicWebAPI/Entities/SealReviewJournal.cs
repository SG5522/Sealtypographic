using SealTypographicWebAPI.Entities.BaseEntities;

namespace SealTypographicWebAPI.Entities
{
    /// <summary>
    /// 客戶印鑑組歷程資料表
    /// </summary>
    public class SealReviewJournal : BaseReviewData
    {
        /// <summary>
        /// 印鑑編號(排序)
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// 印鑑季度
        /// </summary>
        public string Quarter { get; set; }
        
        ///// <summary>
        ///// 印鑑Id
        ///// </summary>
        //public int CustomerSealJournalId { get; set; }

        /// <summary>
        /// 客戶印鑑歷程表
        /// </summary>
        public CustomerSealJournal? CustomerSealJournal { get; set; }

        /// <summary>
        /// 會計簽印歷程表
        /// </summary>
        public AccountantSignJournal? AccountantSignJournal { get; set; }

        /// <summary>
        /// 信頭圖片歷程表
        /// </summary>
        public LetterheadImageJournal? LetterheadImageJournal { get; set; }

    }
}
