using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Entities.BaseEntities;

namespace SealTypographicWebAPI.Entities
{
    /// <summary>
    /// 客戶印鑑組歷程資料表
    /// </summary>
    public class CustomerSealJournal : BaseSealJournal
    {
        /// <summary>
        /// 印鑑配置類別
        /// </summary>
        public CustomerSealConfigType ConfigType { get; set; }

        /// <summary>
        /// 客戶ID
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// 客戶資料表
        /// </summary>
        public Customer Customer { get; set; }

        /// <summary>
        /// 印鑑組歷程資料表
        /// </summary>
        public SealReviewJournal SealReviewJournal { get; set; }

    }
}
