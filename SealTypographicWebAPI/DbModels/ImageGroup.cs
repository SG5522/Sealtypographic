namespace SealTypographicWebAPI.DbModels
{
    /// <summary>
    /// 圖片群組資料表 (使用印鑑、簽名、LOGO)
    /// </summary>
    public class ImageGroup
    {
        /// <summary>
        /// 群組ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 群組名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type(客戶、會計師、信頭)
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 說明
        /// </summary>
        public string Description { get; set; }

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
