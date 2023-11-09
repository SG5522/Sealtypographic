using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.LogReport
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomerTypoReportSearch : PaginateWithDateSearch
    {
        /// <summary>
        /// 關鍵字
        /// </summary>
        public string? Key { get; set; }
    }
}
