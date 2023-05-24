using DBEntitiesExtension.Base;

namespace DBEntitiesExtension
{
    /// <summary>
    /// 臨時章群組
    /// </summary>
    public class TemporarySealGroup : BaseData
    {
        /// <summary>
        /// 臨時章季度
        /// </summary>
        public Quarter Quarter { get; set; }

        /// <summary>
        /// 客戶資料表
        /// </summary>
        public Customer Customer { get; set; }

        /// <summary>
        /// 排版素材
        /// </summary>
        public List<TypographicResource> TypographicResources { get; set; }
    }
}
