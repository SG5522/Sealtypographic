using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.TemporarySeal
{
    /// <summary>
    /// 臨時章分頁搜尋(排板使用)
    /// </summary>
    public class TypographicTemporarySealSearch : PaginateSearch
    {
        /// <summary>
        /// 客戶Id
        /// </summary>
        /// <example>映像公司</example>
        public int CustomerId { get; set; }
        
    }
}
