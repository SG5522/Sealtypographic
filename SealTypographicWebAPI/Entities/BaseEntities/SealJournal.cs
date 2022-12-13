namespace SealTypographicWebAPI.Entities.BaseEntities
{
    /// <summary>
    /// 各項印鑑(簽名)歷程
    /// </summary>
    public class SealJournal : BaseReviewData
    {
        /// <summary>
        /// 圖檔路徑
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// 圖片群組ID 
        /// (目前暫定)
        /// 5.會計印鑑
        /// 6.中文簽名
        /// 7.英文簽名
        /// 8.舊式簽名    
        /// </summary>
        public int SealMappingConfigId { get; set; }

        /// <summary>
        /// 圖片群組資料表 (使用印鑑、簽名、LOGO)
        /// </summary>
        public SealMappingConfig SealMappingConfig { get; set; }
    }
}
