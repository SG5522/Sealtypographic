using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.TemporarySeal
{
    /// <summary>
    /// 臨時章分頁搜尋
    /// </summary>
    public class TemporarySealSearch : PaginateSearch
    {
        /// <summary>
        /// 客戶Id (排板時使用)
        /// </summary>
        public int? CustomerId { get; set; }

        /// <summary>
        /// 關鍵字搜尋 (客戶名稱)
        /// </summary>
        /// <example>映像公司</example>
        public string? KeyWord { get; set; }
        
    }
}
