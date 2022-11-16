namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 客戶分頁搜尋
    /// </summary>
    public class CustomerQueryPage
    {
        /// <summary>
        /// 搜尋客戶ID或是名字
        /// </summary>
        public string? CustomerIdOrName { get; set; }
        
        /// <summary>
        /// 現在頁數(不得小於0)
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// 每頁資料筆數(不得小於0)
        /// </summary>
        public int PageSize { get; set; }
    }
}
