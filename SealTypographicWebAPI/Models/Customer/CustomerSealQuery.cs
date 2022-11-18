namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 客戶分頁搜尋
    /// </summary>
    public class CustomerSealQuery
    {
        /// <summary>
        /// 客戶ID
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// 印鑑季度
        /// </summary>
        public string Quarter { get; set; }
    }
}
