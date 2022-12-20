namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 客戶印鑑分類
    /// </summary>
    public class CustomerSealMappingGroup
    {
        /// <summary>
        /// 客戶ID
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// 印鑑簽印圖片群組ID 
        /// </summary>
        public int SealMappingConfigId { get; set; }

    }
}
