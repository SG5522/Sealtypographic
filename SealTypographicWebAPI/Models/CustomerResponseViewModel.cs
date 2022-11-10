
namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// 客戶資料
    /// </summary>
    public class CustomerResponseViewModel : Response
    {
        /// <summary>
        /// 現在頁數
        /// </summary>
        public int ThisPage { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPage { get; set; }
        
        /// <summary>
        /// 資料筆數
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 顧客查詢表
        /// </summary>
        public List<CustomerViewModel> Customers { get; set; }
    }
}
