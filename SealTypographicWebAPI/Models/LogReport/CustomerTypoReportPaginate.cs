using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.LogReport
{
    /// <summary>
    /// 客戶財稅報排版紀錄列表
    /// </summary>
    public class CustomerTypoReportPaginate : PaginateViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public List<CustomerTypoReportViewModel> ViewModels { get; set; }
    }
}
