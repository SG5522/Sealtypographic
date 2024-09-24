using SealTypographicWebAPI.Models.BaseModels;
using SealTypographicWebAPI.Models.CustomerSealReview;

namespace SealTypographicWebAPI.Models.AccountantSignReview
{
    /// <summary>
    /// 會計師簽印審核檢視列表(分頁)
    /// </summary>
    public class AccountantSignReviewPaginate : PaginateViewModel
    {
        /// <summary>
        /// New ViewModels
        /// </summary>
        public AccountantSignReviewPaginate()
        {
            ViewModels = new();
        }

        /// <summary>
        /// 審核檢視列表
        /// </summary>
        public List<AccountantSignReviewViewModel> ViewModels { get; set; }
    }
}
