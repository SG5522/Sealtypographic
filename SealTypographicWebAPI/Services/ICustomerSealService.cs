using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// 客戶印鑑管理
    /// </summary>
    public interface ICustomerSealService
    {
        /// <summary>
        /// 取得客戶印鑑季度表
        /// </summary>
        /// <param name="customerID">客戶ID</param>        
        /// <returns></returns>
        CustomerSealQuarterResponse GetQuarter(int customerID);

        /// <summary>
        /// 取得客戶印鑑季度表 (排版使用)
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        CustomerSealQuarterResponse GetQuarterWithTypographic(int customerId);

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
        /// <returns></returns>
        Task<ResponseViewModel> New(CustomerSealForm customerSealForms);

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
