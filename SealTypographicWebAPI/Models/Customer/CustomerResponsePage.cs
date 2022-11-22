namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 依搜尋結果與分頁顯示客戶列表
    /// </summary>
    public class CustomerResponsePage : Response
    {
        /// <summary>
        /// 現在頁數
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// 每頁資料筆數(不得小於0)
        /// </summary>
        /// <example>5</example>
        public int PageSize { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPage { get; set; }

        /// <summary>
        /// 資料筆數
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 顧客列表
        /// </summary>
        public List<CustomerViewModel> Customers { get; set; }
    }
}
