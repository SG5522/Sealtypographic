namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 客戶季度搜尋
    /// </summary>
    public class CustomerSealQuarter
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

    /// <summary>
    /// 客戶季度搜尋表
    /// </summary>
    public class CustomerSealQuarters : Response
    {
        /// <summary>
        /// 客戶季度搜尋表
        /// </summary>
        public List<CustomerSealQuarter> Quarters { get; set; }
    }
}
