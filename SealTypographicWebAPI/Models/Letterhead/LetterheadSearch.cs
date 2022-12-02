namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 客戶分頁搜尋
    /// </summary>
    public class LetterheadSearch : PaginateSearchWithStatus
    {
        /// <summary>
        /// 搜尋客戶ID或是名字
        /// </summary>
        /// <example>AAA001 or 公司</example>
        public string? LetterheadIdOrName { get; set; }
    }
}
