using DBEntities.Base;

namespace DBEntities
{
    /// <summary>
    /// 客戶資料表
    /// </summary>        
    public class Customer : BaseDetail
    {
        /// <summary>
        /// 會計師事務所
        /// </summary>
        public Company Company { get; set; }

        /// <summary>
        /// PDF排版資訊
        /// </summary>
        public List<TypographicPDF> TypographicPDFs { get; set; }

        /// <summary>
        /// 客戶印鑑群組歷程表
        /// </summary>
        public List<CustomerSealGroup> CustomerSealGroups { get; set; }


        public List<TemporarySealGroup> TemporarySealGroups { get; set; }
    }
}
