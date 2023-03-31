using DBEntities.Base;

namespace DBEntities
{

    /// <summary>
    /// 客戶印鑑排版位置
    /// </summary>
    public class CustomerSealLocation : BasePageLocation
    {
        /// <summary>
        /// 客戶印鑑歷程
        /// </summary>
        public CustomerSealJournal CustomerSealJournal { get; set; }
    }
}
