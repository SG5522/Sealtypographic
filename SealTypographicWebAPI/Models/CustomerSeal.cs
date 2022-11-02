namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// 客戶章
    /// </summary>
    public class CustomerSeal
    {
        /// <summary>
        /// 印鑑ID
        /// </summary>
        public string? ID { get; set; }
        /// <summary>
        /// 客戶ID
        /// </summary>
        public string? CustomerID { get; set; }
        /// <summary>
        /// 客戶群組ID
        /// </summary>
        public string? CustomerSealGroupsID { get; set; }
        /// <summary>
        /// 圖檔路徑
        /// </summary>
        public string? ImagePath { get; set; }
    }
}
