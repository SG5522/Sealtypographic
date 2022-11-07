namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// 客戶印鑑組
    /// </summary>
    public class CustomerSeal
    {
        /// <summary>
        /// 客戶ID
        /// </summary>
        public string CustomerID { get; set; } = null!;

        /// <summary>
        /// 客戶印鑑群組ID 
        /// (目前暫定)
        /// 1.公司章
        /// 2.負責人
        /// 3.經理
        /// 4.會計主管    
        /// </summary>
        public int CustomerSealGroupsID { get; set; }

        /// <summary>
        /// 圖檔路徑
        /// </summary>
        public string? ImagePath { get; set; }
        
        /// <summary>
        /// 啟用日(審查通過才有)
        /// </summary>
        public DateTime AvailableDate { get; set; }
        
        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// 印鑑季度
        /// </summary>
        public string Quarter { get; set; } = null!;
    }
}
