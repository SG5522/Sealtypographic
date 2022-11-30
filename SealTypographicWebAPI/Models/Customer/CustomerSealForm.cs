namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 客戶印鑑組
    /// </summary>
    public class CustomerSealForm
    {
        /// <summary>
        /// 客戶ID
        /// </summary>
        /// <example>aaa001</example>
        public string CustomerId { get; set; } = null!;

        /// <summary>
        /// 客戶印鑑群組ID 
        /// (目前暫定)
        /// 1.公司章
        /// 2.負責人
        /// 3.經理
        /// 4.會計主管    
        /// </summary>
        /// <example>1</example>
        public int SealMappingConfigId { get; set; }

        /// <summary>
        /// 印鑑編號(排序) 1為起始
        /// </summary>
        /// <example>1</example>
        public int No { get; set; }

        /// <summary>
        /// 圖檔字串(Base64)
        /// </summary>
        /// <example>image/...</example>
        public string ImageBase64 { get; set; }

        /// <summary>
        /// 啟用日(審查通過才有正確時間)
        /// </summary>
        /// <example>0000-01-01T00:00:00.000Z</example>
        public DateTime AvailableDate { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 印鑑季度
        /// </summary>
        /// <example>111年Q1</example>
        public string Quarter { get; set; } = null!;
    }
}
