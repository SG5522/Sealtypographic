namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 客戶分頁搜尋
    /// </summary>
    public class CustomerQuery
    {
        /// <summary>
        /// 搜尋客戶ID或是名字
        /// </summary>
        /// <example>AAA001 or 公司</example>
        public string? CustomerIdOrName { get; set; }

        /// <summary>
        /// 現在頁數(不得小於0)
        /// </summary>
        /// <example>1</example>
        public int PageNumber { get; set; }

        /// <summary>
        /// 每頁資料筆數(不得小於0)
        /// </summary>
        /// <example>5</example>
        public int PageSize { get; set; }
    }
}
