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
        /// <param name="customerSealReviewSearch">客戶印鑑審核狀態分頁搜尋</param>
        /// <param name="typographyType">排版類別</param>
        /// <param name="userId">登入的使用者Id</param>
        /// <returns></returns>
        CustomerSealGroupReviewPaginate GetReviewList(CustomerSealSearchReview customerSealReviewSearch, TypographyType typographyType, int userId = 1);

        /// <summary>
        /// 基本資料與印鑑細項
        /// </summary>
        /// <param name="CustomerSealQuarterId">印鑑季度Id</param>
        /// <param name="userId">登入的使用者Id</param>
        /// <returns></returns>
        CustomerSealGroupDetailReviewResponse GetReviewDetail(int CustomerSealQuarterId, int userId = 1);

        /// <summary>
        /// 更換審核狀態
        /// </summary>
        /// <param name="customerSealQuarterIds">審核季度Id</param>
        /// <param name="reviewStatus">審核狀態</param>
        /// <param name="userId">登入的使用者Id</param>
        /// <returns></returns>
        ResponseViewModel StatusChange(List<int> customerSealQuarterIds, ReviewStatus reviewStatus, int userId = 1);
    }
}
