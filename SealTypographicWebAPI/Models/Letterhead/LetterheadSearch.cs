using SealTypographicWebAPI.Models.PublicModel;

namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 客戶分頁搜尋
    /// </summary>
    public class LetterheadSearch : PaginateSearchWithReviewStatus
    {
        /// <summary>
        /// 搜尋名字
        /// </summary>
        /// <example>信頭名稱</example>
        public string? Name { get; set; }
    }
}
