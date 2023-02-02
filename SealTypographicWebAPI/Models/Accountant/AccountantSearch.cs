using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 會計師分頁搜尋
    /// </summary>
    public class AccountantSearch : PaginateSearch
    {
        /// <summary>
        /// 關鍵字搜尋
        /// </summary>        
        /// <example>ACC001 or 王XX or 台北群組</example>
        public string? KeyWord { get; set; }        
    }
}
