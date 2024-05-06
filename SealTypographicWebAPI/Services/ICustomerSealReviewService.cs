using DBEntities.Consts;
using SealTypographicWebAPI.Models;
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
        /// <param name="customerSealSearchReview">客戶印鑑審核狀態分頁搜尋</param>
        /// <param name="typographyType">排版類別</param>
        /// <param name="userInfo">登入使用者基本資訊</param>
        /// <returns></returns>        
        Task<CustomerSealGroupReviewPaginate> GetReviewList(CustomerSealSearchReview customerSealSearchReview, TypographyType typographyType, UserInfo userInfo);

        /// <summary>
        /// 基本資料與印鑑細項
        /// </summary>
        /// <param name="customerSealQuarterId">印鑑季度Id</param>
        /// <param name="userInfo">登入使用者基本資訊</param>
        /// <returns></returns>        
        Task<CustomerSealGroupDetailReviewResponse> GetReviewDetail(int customerSealQuarterId, UserInfo userInfo);

        /// <summary>
        /// 更換審核狀態
        /// </summary>
        /// <param name="customerSealQuarterIds">審核季度Id</param>
        /// <param name="reviewStatus">審核狀態</param>
        /// <param name="userInfo">登入使用者基本資訊</param>
        /// <returns></returns>        
        Task<ResponseViewModel> StatusChange(List<int> customerSealQuarterIds, ReviewStatus reviewStatus, UserInfo userInfo);
    }
}
