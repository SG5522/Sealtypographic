
namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// 客戶資料
    /// </summary>
    public class CustomerResponseViewModel : Response
    {
        /// <summary>
        /// 顧客查詢表
        /// </summary>
        public List<CustomerViewModel> Customers { get; set; }
    }
}
