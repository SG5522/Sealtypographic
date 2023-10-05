using DBEntities.Base;
using DBEntities.Consts;

namespace DBEntities
{
    /// <summary>
    /// 客戶印鑑季度資料表
    /// </summary>
    public class CustomerSealGroup : BaseReviewData
    {
        /// <summary>
        /// 季度
        /// </summary>
        public QuarterYear QuarterYear { get; set; }

        public TypographyType TypographyType { get; set; }

        /// <summary>
        /// 客戶基本資料表
        /// </summary>
        public Customer Customer { get; set; }

        /// <summary>
        /// 排版素材
        /// </summary>
        public IList<TypographicResource> TypographicResources { get; set; }
    }
}
