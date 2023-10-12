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
        /// <param name="isTypographicUse">是否給排版使用</param>
        /// <param name="typographyType">排版類別</param>  
        /// <returns></returns>
        CustomerPaginateViewModel GetPaginate(CustomerSearch customerSearch, bool isTypographicUse, TypographyType typographyType);

        /// <summary>
        /// 取得客戶印鑑季度表(分頁)
        /// </summary>
        /// <param name="customerSealQuarterPaginateSearch">印鑑季度分頁搜尋</param>
        /// <param name="isTypographic">是否排版使用</param>
        /// <returns></returns>
        CustomerSealQuarterPaginateViewModel GetQuarter(CustomerSealQuarterPaginateSearch customerSealQuarterPaginateSearch, bool isTypographic);

        /// <summary>
        /// 取得印鑑群組簡易資訊
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="quaterId"></param>
        /// <returns></returns>
        CustomerSealGroupResponse GetCustomerSealGroupSummry(int customerId, int quaterId);

        /// <summary>
        /// 取得客戶印鑑組
        /// </summary>
        /// <param name="customerSealQuarterId">客戶印鑑季度Id</param>
        /// <param name="isTransparent">是否白底透明化</param>
        /// <returns></returns>
        CustomerSealViewModels GetSeals(int customerSealQuarterId, bool isTransparent);

        /// <summary>
        /// 新增客戶印鑑組資料
        /// </summary>
        /// <param name="customerSealForms">客戶印鑑組資料</param>
        /// <param name="typographyType">排版類別</param>
        /// <returns></returns>
        Task<ResponseViewModel> New(CustomerSealForm customerSealForms, TypographyType typographyType);

        /// <summary>
        /// 異動客戶印鑑
        /// </summary>
        /// <param name="customerSealUpdate">需要異動客戶印鑑資料</param>
        /// <returns></returns>        
        Task<List<ResponseViewModel>> Update(CustomerSealUpdate customerSealUpdate);

        /// <summary>
        /// 此季度印鑑從草稿狀態變更為待審
        /// </summary>
        /// <param name="customerSealQuarterId">客戶印鑑季度Id</param>
        /// <returns></returns>
        ResponseViewModel Pending(int customerSealQuarterId);

        /// <summary>
        /// 此季度印鑑從草稿狀態變更為作廢
        /// </summary>
        /// <param name="customerSealQuarterId">客戶印鑑季度Id</param>
        /// <returns></returns>
        ResponseViewModel Invalid(int customerSealQuarterId);

        /// <summary>
        /// 此季度印鑑從待審狀態變更為草稿
        /// </summary>
        /// <param name="customerSealQuarterId">客戶印鑑季度Id</param>        
        ResponseViewModel CancelReview(int customerSealQuarterId);
    }
}
