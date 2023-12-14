using DBEntities.Entities.Base;
using DBEntities.Entities.CustomerModels;
using DBEntities.Entities.TypographicModels;

namespace DBEntities.Entities.TemplateModels
{
    /// <summary>
    /// 臨時章群組
    /// </summary>
    public class TemporarySealGroup : BaseData
    {
        /// <summary>
        /// 臨時章季度
        /// </summary>
        public QuarterYear QuarterYear { get; set; }

        /// <summary>
        /// 客戶資料表
        /// </summary>
        public Customer Customer { get; set; }

        /// <summary>
        /// 排版素材
        /// </summary>
        public IList<TypographicResource> TypographicResources { get; set; }
    }
}
