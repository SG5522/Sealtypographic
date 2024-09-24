using DBEntities.Consts;
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
        /// <param name="userInfo">登入使用者基本資訊</param>        
        /// <returns></returns>
        Task<AccountantSignReviewPaginate> GetReviewPaginate(AccountantSignSearchReview accountantSignSearchReview, UserInfo userInfo);

        /// <summary>
        /// 基本資料與簽印組
        /// </summary>
        /// <param name="accountantSignGroupId">會計師簽印組Id</param>
        /// <param name="userInfo">登入使用者基本資訊</param>
        /// <returns></returns>
        Task<AccountantSignDetailReviewResponse> GetReviewDetail(int accountantSignGroupId, UserInfo userInfo);

        /// <summary>
        /// 更換審核狀態
        /// </summary>
        /// <param name="accountantSignGroupIds">會計師簽印群組Id</param>
        /// <param name="reviewStatus">審核狀態</param>
        /// <param name="userInfo">登入使用者基本資訊</param>        
        /// <returns></returns>
        Task<ResponseViewModel> StatusChange(List<int> accountantSignGroupIds, ReviewStatus reviewStatus, UserInfo userInfo);
    }
}
