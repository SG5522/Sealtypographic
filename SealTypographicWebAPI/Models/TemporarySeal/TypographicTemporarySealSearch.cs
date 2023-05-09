using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.TemporarySeal
{
    /// <summary>
    /// 臨時章分頁搜尋
    /// </summary>
    public class TypographicTemporarySealSearch : PaginateSearch
    {
        /// <summary>
        /// 客户Id
        /// </summary>
        /// <example>映像公司</example>
        public int CustomerId { get; set; }
        
    }
}
