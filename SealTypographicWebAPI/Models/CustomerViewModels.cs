
namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// 客戶資料
    /// </summary>
    public class CustomerViewModels : Response
    {
        /// <summary>
        /// 顧客查詢表
        /// </summary>
        public List<CustomerViewModel>? ViewModels { get; set; }        
    }
}
