using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.AccountantSignReview;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// 客戶印鑑審核管理
    /// </summary>
    public interface IAccountantSignReviewService
    {
        /// <summary>
        /// 待審清單
        /// </summary>
        /// <param name="accountantSignSearchReview">會計師簽印審核狀態分頁搜尋</param>
        /// <returns></returns>
        AccountantSignGroupReviewPaginate GetReviewPaginate(AccountantSignSearchReview accountantSignSearchReview);

        /// <summary>
        /// 基本資料與簽印組
        /// </summary>
        /// <param name="accountantSignGroupId">會計師簽印組Id</param>
        /// <returns></returns>
        AccountantSignGroupDetailReviewResponse GetReviewDetail(int accountantSignGroupId);

        /// <summary>
        /// 審核通過
        /// </summary>
        /// <param name="accountantSignGroupIds">需要更新的ID</param>
        /// <returns></returns>
        ResponseViewModel Approval(List<int> accountantSignGroupIds);

        /// <summary>
        /// 審核退件
        /// </summary>
        /// <param name="accountantSignGroupIds"></param>
        /// <returns></returns>
        ResponseViewModel Reject(List<int> accountantSignGroupIds);

        /// <summary>
        /// 審核不受理
        /// </summary>
        /// <param name="accountantSignGroupIds"></param>
        /// <returns></returns>
        ResponseViewModel Refuse(List<int> accountantSignGroupIds);

    }
}
