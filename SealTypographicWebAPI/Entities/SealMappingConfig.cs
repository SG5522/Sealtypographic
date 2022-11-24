using Microsoft.EntityFrameworkCore;

namespace SealTypographicWebAPI.Entities
{
    /// <summary>
    /// 印鑑Mapping設定表 (使用印鑑、簽名、LOGO)
    /// </summary>
    [Index(nameof(SubId), IsUnique = true)]
    public class SealMappingConfig
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// SealType(客戶、會計師、信頭)
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// SubId
        /// </summary>        
        public string SubId { get; set; }

        /// <summary>
        /// 印鑑類別名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 客戶印鑑組歷程資料表
        /// </summary>
        public List<CustomerSealJournal> CustomerSealJournals { get; set; }

        /// <summary>
        /// 會計師印鑑簽名組歷程資料表
        /// </summary>
        public List<AccountantSignJournal> AccountantSignJournals { get; set; }

        /// <summary>
        /// 事務所信頭圖片歷程資料表
        /// </summary>
        public List<LetterheadImageJournal> LetterheadImageJournals { get; set; }
    }
}
