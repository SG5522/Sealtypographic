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

        /// <summary>
        /// 印鑑歷程id
        /// </summary>
        public int SealJournalId { get; set; }

        /// <summary>
        /// 客戶資料表
        /// </summary>
        public SealJournal SealJournal { get; set; }

    }
}
