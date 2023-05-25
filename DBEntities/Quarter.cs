using DBEntities.Base;

namespace DBEntities
{
    /// <summary>
    /// 季度
    /// </summary>
    public class Quarter : BaseData
    {
        /// <summary>
        /// 西元年(公曆)
        /// </summary>
        public string GregorianYear { get; set; }

        /// <summary>
        /// 民國年
        /// </summary>
        public string TaiwanYear { get; set; }

        /// <summary>
        /// 季度時期(Q1,Q2,Q3,Q4)
        /// </summary>
        public string Period { get; set; }

        /// <summary>
        /// 客戶印鑑季度資料表
        /// </summary>
        public List<CustomerSealGroup> CustomerSealGroups { get; set; }

        /// <summary>
        /// 臨時章群組
        /// </summary>
        public List<TemporarySealGroup> TemporarySealGroups { get; set; }

        /// <summary>
        /// PDF排版資訊
        /// </summary>
        public List<TypographicPDF> TypographicPDFs { get; set; }
    }
}
