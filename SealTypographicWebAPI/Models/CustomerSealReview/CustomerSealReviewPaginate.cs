using DBEntities.Consts;
using SealTypographicWebAPI.Models.BaseModels;
using SealTypographicWebAPI.Models.Customer;

namespace SealTypographicWebAPI.Models.CustomerSealReview
{
    /// <summary>
    /// 客戶印鑑審核檢視列表(分頁)
    /// </summary>
    public class CustomerSealReviewPaginate : PaginateViewModel
    {
        /// <summary>
        /// New ViewModels
        /// </summary>
        public CustomerSealReviewPaginate()
        {
            ViewModels = new();
        }

        /// <summary>
        /// 審核檢視列表
        /// </summary>
        public List<CustomerSealReviewViewModel> ViewModels { get; set; }
    }
}
