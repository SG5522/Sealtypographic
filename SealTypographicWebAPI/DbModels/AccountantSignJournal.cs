namespace SealTypographicWebAPI.DbModels
{
    /// <summary>
    /// 會計師印鑑簽名組歷程資料表
    /// </summary>
    public class AccountantSignJournal
    {
        /// <summary>
        /// 會計印鑑簽名組ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 圖檔路徑
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// 啟用日(審查通過才有)
        /// </summary>
        public DateTime AvailableDate { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 會計師ID
        /// </summary>
        public string AccountantId { get; set; } = null!;

        /// <summary>
        /// 會計師資料表
        /// </summary>
        public Accountant Accountant { get; set; }

        /// <summary>
        /// 圖片群組ID 
        /// (目前暫定)
        /// 5.會計印鑑
        /// 6.中文簽名
        /// 7.英文簽名
        /// 8.舊式簽名    
        /// </summary>
        public int ImageGroupId { get; set; }

        /// <summary>
        /// 圖片群組資料表 (使用印鑑、簽名、LOGO)
        /// </summary>
        public ImageGroup ImageGroup { get; set; }
    }
}
