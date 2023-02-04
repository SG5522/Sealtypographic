using SealTypographicWebAPI.Models.CustomerSealReview;

namespace SealTypographicWebAPI.Models.AccountantSignReview
{
    /// <summary>
    /// 會計師簽印審核檢視列表(分頁)
    /// </summary>
    public class AccountantSignGroupReviewPaginate : ResponseViewModel
    {
        /// <summary>
        /// New ViewModels
        /// </summary>
        public AccountantSignGroupReviewPaginate()
        {
            ViewModels = new();
        }

        /// <summary>
        /// 審核檢視列表
        /// </summary>
        public List<AccountantSignGroupReviewViewModel> ViewModels { get; set; }
    }
}
