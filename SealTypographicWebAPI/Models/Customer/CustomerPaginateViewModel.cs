namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 依搜尋結果與分頁顯示客戶列表
    /// </summary>
    public class CustomerPaginateViewModel : PaginateViewModel
    {
        /// <summary>
        /// 顧客列表
        /// </summary>
        public List<CustomerViewModel> Customers { get; set; }
    }
}
