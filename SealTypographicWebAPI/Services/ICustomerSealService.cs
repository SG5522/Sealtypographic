using DBEntities.Consts;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Models.CustomerSeal;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// 客戶印鑑管理
    /// </summary>
    public interface ICustomerSealService
    {
        /// <summary>
        /// 取得客戶列表(分頁)
        /// 此列表參照是否有印鑑搜尋
        /// 分為財報印鑑、稅報印鑑
        /// </summary>
        /// <param name="customerSearch">客戶分頁搜尋</param>        
        /// <param name="typographyType">排版類別</param>
        /// <param name="userId">登入的使用者Id</param>  
        /// <returns></returns>
        Task<CustomerPaginateViewModel> GetPaginate(CustomerSearch customerSearch, TypographyType typographyType, int userId = 1);

        /// <summary>
        /// 取得客戶印鑑季度表(分頁)
        /// </summary>
        /// <param name="customerSealQuarterPaginateSearch">印鑑季度分頁搜尋</param>
        /// <param name="isTypographic">是否排版使用</param>
        /// <param name="typographyType">排版類別</param>
        /// <param name="userId">登入的使用者Id</param>
        /// <returns></returns>
        Task<CustomerSealQuarterPaginateViewModel> GetQuarterYear(CustomerSealQuarterPaginateSearch customerSealQuarterPaginateSearch
            , bool isTypographic, TypographyType typographyType, int userId = 1);

        /// <summary>
        /// 取得印鑑群組簡易資訊
        /// </summary>
        /// <param name="customerId">客戶Id</param>
        /// <param name="quaterId">年季度Id</param>
        /// <param name="userId">登入的使用者Id</param>
        /// <returns></returns>
        Task<CustomerSealGroupResponse> GetCustomerSealGroupSummry(int customerId, int quaterId, int userId = 1);

        /// <summary>
        /// 取得客戶印鑑組
        /// </summary>
        /// <param name="customerSealQuarterId">客戶印鑑季度Id</param>
        /// <param name="isTransparent">是否白底透明化</param>
        /// <param name="userInfo">登入使用者基本資訊</param>        
        /// <returns></returns>        
        Task<CustomerSealViewModels> GetSeals(int customerSealQuarterId, bool isTransparent, UserInfo userInfo);

        /// <summary>
        /// 新增客戶印鑑組資料
        /// </summary>
        /// <param name="customerSealForm">客戶印鑑組資料</param>
        /// <param name="typographyType">排版類別</param>
        /// <param name="userInfo">登入使用者基本資訊</param>
        /// <returns></returns>        
        Task<ResponseViewModel> New(CustomerSealForm customerSealForm, TypographyType typographyType, UserInfo userInfo);

        /// <summary>
        /// 異動客戶印鑑
        /// </summary>
        /// <param name="customerSealUpdate">需要異動客戶印鑑資料</param>
        /// <param name="userId">登入的使用者Id</param>
        /// <returns></returns>
        Task<List<ResponseViewModel>> Update(CustomerSealUpdate customerSealUpdate, int userId = 1);


        /// <summary>
        /// 客戶印鑑群組狀態變更
        /// </summary>
        /// <param name="customerSealQuarterId">客戶印鑑季度Id</param>
        /// <param name="reviewStatus">審查狀態</param>
        /// <param name="userInfo">登入使用者基本資訊</param>
        Task<ResponseViewModel> ChangeReviewStatus(int customerSealQuarterId, ReviewStatus reviewStatus, UserInfo userInfo);        
    }
}
