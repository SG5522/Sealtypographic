using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.LogReport.TypographicReport
{
    /// <summary>
    /// 客戶財稅報排版紀錄列表
    /// </summary>
    public class TypographicReportPaginate : PaginateViewModel
    {
        /// <summary>
        /// 客戶財稅報排版紀錄
        /// </summary>
        public List<TypographicReportViewModel> ViewModels { get; set; }
    }
}
