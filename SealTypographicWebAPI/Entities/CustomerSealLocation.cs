using SealTypographicWebAPI.Entities.BaseEntities;

namespace SealTypographicWebAPI.Entities
{

    /// <summary>
    /// 客戶印鑑排版位置
    /// </summary>
    public class CustomerSealLocation : Location
    {
        /// <summary>
        /// 客戶印鑑ID
        /// </summary>
        public int CustomerSealJournalId { get; set; }

        /// <summary>
        /// 客戶印鑑歷程
        /// </summary>
        public CustomerSealJournal CustomerSealJournal { get; set; }
    }
}
