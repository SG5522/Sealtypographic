using NetTopologySuite.IO;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models.BaseModels;
using SealTypographicWebAPI.Models.Customer;

namespace SealTypographicWebAPI.Models.CustomerSealReview
{
    /// <summary>
    /// 客戶印鑑審核檢視列表(分頁)
    /// </summary>
    public class CustomerSealQuarterReviewPaginate : PaginateViewModel
    {
        /// <summary>
        /// New ViewModels
        /// </summary>
        public CustomerSealQuarterReviewPaginate()
        {
            ViewModels = new();
        }

        /// <summary>
        /// 審核檢視列表
        /// </summary>
        public List<CustomerSealQuarterReviewViewModel> ViewModels { get; set; }
    }
}
