namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 客戶印鑑組
    /// </summary>
    public class CustomerSeals : Response
    {        
        /// <summary>
        /// 印鑑組
        /// </summary>
        public List<CustomerSeal>? Seals { get; set; }
    }
}
