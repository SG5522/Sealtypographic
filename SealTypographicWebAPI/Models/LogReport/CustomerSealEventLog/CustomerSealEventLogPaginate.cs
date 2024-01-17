using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.LogReport.CustomerSealEventLog
{
    /// <summary>
    /// 客戶印鑑異動列表
    /// </summary>
    public class CustomerSealEventLogPaginate : PaginateViewModel
    {
        /// <summary>
        /// 列表內容
        /// </summary>
        public List<CustomerSealEventLogViewModel> ViewModels { get; set; }
    }

}
