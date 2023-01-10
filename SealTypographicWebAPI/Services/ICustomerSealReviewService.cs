using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
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

        /// <summary>
        /// 基本資料與印鑑細項
        /// </summary>
        /// <param name="customerSealQuarter"></param>
        /// <returns></returns>
        CustomerSealReviewDetailResponse GetCustomerSealReviewDetail(CustomerSealQuarter customerSealQuarter);

        /// <summary>
        /// 審核通過
        /// </summary>
        /// <param name="customerSealIds">需要更新的ID</param>
        /// <returns></returns>
        ResponseViewModel ReviewApproval(List<int> customerSealIds);

        /// <summary>
        /// 審核退件
        /// </summary>
        /// <param name="customerSealIds"></param>
        /// <returns></returns>
        ResponseViewModel ReviewReject(List<int> customerSealIds);

    }
}
