using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.LogReport
{
    /// <summary>
    /// 
    /// </summary>
    public class TypographicReportSearch : PaginateWithDateSearch
    {
        /// <summary>
        /// 使用者名稱
        /// (排版此檔的使用者)
        /// </summary>
        public string? UserKeyWord { get; set; }

        /// <summary>
        /// 使用者名稱
        /// (排版此檔的使用者)
        /// </summary>
        public string? CustomerKeyWord { get; set; }
    }
}
