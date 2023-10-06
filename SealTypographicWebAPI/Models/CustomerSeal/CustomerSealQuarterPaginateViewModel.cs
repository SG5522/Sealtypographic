using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.CustomerSeal
{
    /// <summary>
    /// 客戶印鑑季度列表(分頁)
    /// </summary>
    public class CustomerSealQuarterPaginateViewModel : PaginateViewModel
    {
        /// <summary>
        /// new CustomerSealQuarterView
        /// </summary>
        public CustomerSealQuarterPaginateViewModel()
        {
            CustomerSealQuarters = new();
        }

        /// <summary>
        /// 客戶季度搜尋表
        /// </summary>
        public List<CustomerSealQuarterViewModel> CustomerSealQuarters { get; set; }
    }
}
