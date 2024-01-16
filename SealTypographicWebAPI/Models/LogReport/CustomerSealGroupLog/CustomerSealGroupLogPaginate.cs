using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.LogReport.SealGroupLog
{
    /// <summary>
    /// 客戶印鑑異動列表
    /// </summary>
    public class CustomerSealGroupLogPaginate : PaginateViewModel
    {
        /// <summary>
        /// 列表內容
        /// </summary>
        public List<CustomerSealGroupLogViewModel> ViewModels { get; set; }
    }

}
