using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Temporary
{
    /// <summary>
    /// 臨時章分頁搜尋
    /// </summary>
    public class TemporarySealSearch : PaginateSearch
    {
        /// <summary>
        /// 關鍵字搜尋
        /// </summary>
        /// <example>公司</example>
        public string? KeyWord { get; set; }       
    }
}
