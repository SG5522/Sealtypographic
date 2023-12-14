using DBEntities.Consts;
using DBEntities.Entities.Base;
using DBEntities.Entities.TypographicModels;

namespace DBEntities.Entities.CustomerModels
{
    /// <summary>
    /// 客戶印鑑季度資料表
    /// </summary>
    public class CustomerSealGroup : BaseReviewData
    {
        /// <summary>
        /// 年季度
        /// </summary>
        public QuarterYear QuarterYear { get; set; }

        /// <summary>
        /// 排版類別
        /// </summary>
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
