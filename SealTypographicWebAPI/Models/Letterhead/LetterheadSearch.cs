using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 客戶分頁搜尋
    /// </summary>
    public class LetterheadSearch : PaginateSearch
    {
        /// <summary>
        /// 搜尋名字
        /// </summary>
        /// <example>信頭名稱</example>
        public string? LetterheadOrName { get; set; }
    }
}
