using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 會計師分頁搜尋
    /// </summary>
    public class AccountantSearch : PaginateSearch
    {
        /// <summary>
        /// 關鍵字搜尋 (排版時不含群組名稱)
        /// </summary>        
        /// <example>ACC001 or 王XX or 台北群組</example>
        public string? KeyWord { get; set; }   
        
        /// <summary>
        /// 會計師群組編號
        /// </summary>
        public string? AccountantGroupNumber { get; set; }
    }
}
