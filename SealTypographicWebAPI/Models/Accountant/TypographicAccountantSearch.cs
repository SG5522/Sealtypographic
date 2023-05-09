using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 會計師分頁搜尋
    /// </summary>
    public class TypographicAccountantSearch : PaginateSearch
    {
        /// <summary>
        /// 關鍵字搜尋(會計師編號與姓名)
        /// </summary>        
        /// <example>ACC001 or 王XX</example>
        public string? KeyWord { get; set; }   
        
        /// <summary>
        /// 會計師群組Id
        /// </summary>
        public int? AccountantGroupId { get; set; }
    }
}
