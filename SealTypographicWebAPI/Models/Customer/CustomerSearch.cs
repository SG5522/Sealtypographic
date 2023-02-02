using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 客戶分頁搜尋
    /// </summary>
    public class CustomerSearch : PaginateSearch
    {
        /// <summary>
        /// 關鍵字搜尋
        /// </summary>
        /// <example>AAA001 or 公司</example>
        public string? KeyWord { get; set; }       
    }
}
