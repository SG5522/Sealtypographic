using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.TemporarySeal
{
    /// <summary>
    /// 臨時章分頁搜尋
    /// </summary>
    public class TemporarySealSearch : PaginateSearch
    {
        /// <summary>
        /// 關鍵字搜尋 (客戶名稱)
        /// </summary>
        /// <example>映像公司</example>
        public string? KeyWord { get; set; }
        
    }
}
