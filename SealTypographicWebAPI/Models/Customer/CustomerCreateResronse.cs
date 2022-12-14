namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 建完客戶後回傳ID
    /// </summary>
    public class CreateCustomerResponse : ResponseViewModel
    {
        /// <summary>
        /// 客戶Id
        /// </summary>        
        public int CustomerId { get; set; }
    }
}
