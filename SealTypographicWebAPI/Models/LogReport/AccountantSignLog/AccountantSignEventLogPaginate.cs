using SealTypographicWebAPI.Models.BaseModels;
using SealTypographicWebAPI.Models.LogReport.CustomerSealEventLog;

namespace SealTypographicWebAPI.Models.LogReport.AccountantSignLog
{
    /// <summary>
    /// 客戶印鑑異動列表
    /// </summary>
    public class AccountantSignEventLogPaginate : PaginateViewModel
    {
        /// <summary>
        /// 列表內容
        /// </summary>
        public List<AccountantSignEventLogViewModel> ViewModels { get; set; }
    }

}
