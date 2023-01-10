using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Entities.BaseEntities;

namespace SealTypographicWebAPI.Entities
{
    /// <summary>
    /// 會計師印鑑簽名組歷程資料表
    /// </summary>
    public class AccountantSignJournal : BaseSealJournal
    {

        /// <summary>
        /// 簽印配置類別
        /// </summary>
        public AccountantSignConfigType ConfigType { get; set; }

        /// <summary>
        /// 會計師ID
        /// </summary>
        public int AccountantId { get; set; }

        /// <summary>
        /// 會計師資料表
        /// </summary>
        public Accountant Accountant { get; set; }

        /// <summary>
        /// 印鑑組歷程資料表
        /// </summary>
        public SealReviewJournal SealReviewJournal { get; set; }
    }
}
