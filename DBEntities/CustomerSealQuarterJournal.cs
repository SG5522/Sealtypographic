using DBEntities.Base;

namespace DBEntities
{
    /// <summary>
    /// 客戶印鑑季度歷程資料表
    /// </summary>
    public class CustomerSealQuarterJournal : BaseReviewData
    {
        /// <summary>
        /// 印鑑季度
        /// </summary>
        public string Quarter { get; set; }

        /// <summary>
        /// 客戶基本資料表
        /// </summary>
        public Customer Customer { get; set; }

        /// <summary>
        /// 客戶印鑑歷程表
        /// </summary>
        public List<CustomerSealJournal> CustomerSealJournals { get; set; }

    }
}
