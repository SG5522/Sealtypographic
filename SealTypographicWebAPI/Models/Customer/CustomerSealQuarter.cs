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
        /// <example>aaa001</example>
        public int CustomerId { get; set; }

        /// <summary>
        /// 印鑑季度
        /// </summary>
        /// <example>111年Q1</example>
        public string Quarter { get; set; }
    }

    /// <summary>
    /// 客戶季度搜尋表
    /// </summary>
    public class CustomerSealQuarters : ResponseViewModel
    {
        /// <summary>
        /// 客戶季度搜尋表
        /// </summary>
        public List<CustomerSealQuarter> Quarters { get; set; }
    }
}
