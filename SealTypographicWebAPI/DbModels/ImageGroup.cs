namespace SealTypographicWebAPI.DbModels
{
    /// <summary>
    /// 圖像群組資料表 (使用印鑑、簽名、LOGO)
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
    }
}
