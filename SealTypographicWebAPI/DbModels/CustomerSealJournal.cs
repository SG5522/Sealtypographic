namespace SealTypographicWebAPI.DbModels
{
    /// <summary>
    /// 客戶印鑑組歷程資料表
    /// </summary>
    public class CustomerSealJournal
    {
        /// <summary>
        /// 客戶印鑑組ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 印鑑編號(排序) 1為起始
        /// </summary>
        public int No { get; set; }

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
        /// 印鑑季度
        /// </summary>
        public string Quarter { get; set; } = null!;

        /// <summary>
        /// 印鑑狀態 
        /// 0.待審查
        /// 1.通過(審核完成)
        /// 2.退件
        /// 3.刪除(系統管理員可以看到資料)
        /// </summary>
        public int Stauts { get; set; } 

        /// <summary>
        /// 客戶ID
        /// </summary>
        public string CustomerId { get; set; } = null!;

        /// <summary>
        /// 客戶資料表
        /// </summary>
        public Customer Customer { get; set; }

        /// <summary>
        /// 圖片群組ID 
        /// (目前暫定)
        /// 1.公司章
        /// 2.負責人
        /// 3.經理
        /// 4.會計主管    
        /// </summary>
        public string SealMappingConfigSubId { get; set; }

        /// <summary>
        /// 圖片群組資料表 (使用印鑑、簽名、LOGO)
        /// </summary>
        public SealMappingConfig SealMappingConfig { get; set; }
    }
}
