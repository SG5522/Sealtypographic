namespace SealTypographicWebAPI.DbModels
{
    /// <summary>
    /// 客戶印鑑組歷程
    /// </summary>
    public class CustomerSealJournal
    {
        /// <summary>
        /// 客戶印鑑組ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 客戶ID
        /// </summary>
        public string CustomerId { get; set; } = null!;

        /// <summary>
        /// 客戶印鑑群組ID 
        /// (目前暫定)
        /// 1.公司章
        /// 2.負責人
        /// 3.經理
        /// 4.會計主管    
        /// </summary>
        public int GroupsId { get; set; }

        /// <summary>
        /// 客戶群組名稱
        /// </summary>
        public int GroupsName { get; set; }

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
    }
}
