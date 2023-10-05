using DBEntities.Base;
using DBEntities.Consts;

namespace DBEntities
{
    /// <summary>
    /// 年季度
    /// </summary>
    public class QuarterYear : BaseData
    {
        /// <summary>
        /// 西元年(公曆)
        /// </summary>
        public int GregorianYear { get; set; }

        /// <summary>
        /// 季度時期(Q1,Q2,Q3,Q4)
        /// </summary>
        public string? Period { get; set; }

        /// <summary>
        /// 年季度類型
        /// </summary>
        public TypographyType Type { get; set; }

        /// <summary>
        /// 客戶印鑑季度資料表
        /// </summary>
        public IList<CustomerSealGroup> CustomerSealGroups { get; set; }

        /// <summary>
        /// 臨時章群組
        /// </summary>
        public IList<TemporarySealGroup> TemporarySealGroups { get; set; }

        /// <summary>
        /// PDF排版資訊
        /// </summary>
        public IList<TypographicPDF> TypographicPDFs { get; set; }
    }
}
