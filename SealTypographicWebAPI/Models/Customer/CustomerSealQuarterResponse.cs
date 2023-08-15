namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 客戶印鑑季度列表
    /// </summary>
    public class CustomerSealQuarterResponse : ResponseViewModel
    {
        /// <summary>
        /// new CustomerSealQuarterView
        /// </summary>
        public CustomerSealQuarterResponse()
        {
            CustomerSealQuarters = new();
        }

        /// <summary>
        /// 客戶季度搜尋表
        /// </summary>
        public List<CustomerSealQuarterViewModel> CustomerSealQuarters { get; set; }
    }
}
