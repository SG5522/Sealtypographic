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
        /// 印鑑編號(排序) 1為起始
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// 印鑑季度
        /// </summary>
        public string Quarter { get; set; }

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

    }
}
