using SealTypographicWebAPI.Models.CustomerSealReview;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// 客戶印鑑審核管理
    /// </summary>
    public interface ICustomerSealReviewService
    {
        /// <summary>
        /// 待審清單
        /// </summary>
        /// <returns></returns>
        CustomerSealReviewViewModelResponse GetCustomerSealReviewViewModel(CustomerSealReviewSearch customerSealReviewSearch);
    }
}
