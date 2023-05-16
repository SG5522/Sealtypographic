using DBEntities.Base;
using DBEntities.Consts;

namespace DBEntities
{
    /// <summary>
    /// 客戶印鑑歷程資料表
    /// </summary>
    public class CustomerSealJournal : BaseSealJournal
    {

        /// <summary>
        /// 印鑑配置類別
        /// </summary>
        public CustomerSealType ConfigType { get; set; }

        /// <summary>
        /// 印鑑編號(排序)
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// 客戶印鑑季度歷程資料表
        /// </summary>
        public CustomerSealQuarterJournal CustomerSealQuarterJournal { get; set; }

        /// <summary>
        /// 各印鑑簽印排版位置
        /// </summary>
        public List<TypographicSealLocation> TypographicSealLocations { get; set; }
    }
}
