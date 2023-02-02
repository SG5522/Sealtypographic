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
        CustomerSealQuarterViews GetQuarter(int customerID);


        /// <summary>
        /// 取得客戶印鑑組
        /// </summary>
        /// <param name="customerSealQuarter">客戶印鑑搜尋(依客戶ID與季度)</param>
        /// <returns></returns>
        CustomerSealViewModels GetSeals(CustomerSealSearchQuarter customerSealQuarter);

        /// <summary>
        /// 新增客戶印鑑組資料
        /// </summary>
        /// <param name="customerSealForms">客戶印鑑組資料</param>
        /// <returns></returns>
        ResponseViewModel New(CustomerSealForm customerSealForms);

        /// <summary>
        /// 異動客戶印鑑
        /// </summary>
        /// <param name="customerSealUpdate">需要異動客戶印鑑資料</param>
        /// <returns></returns>        
        List<ResponseViewModel> Update(CustomerSealUpdate customerSealUpdate);

        /// <summary>
        /// 此季度印鑑從草稿狀態變更為待審
        /// </summary>
        /// <param name="customerSealQuarter">客戶印鑑搜尋(依客戶ID與季度)</param>
        /// <returns></returns>
        ResponseViewModel PendingSeals(CustomerSealSearchQuarter customerSealQuarter);

        /// <summary>
        /// 此季度印鑑從草稿狀態變更為作廢
        /// </summary>
        /// <param name="customerSealQuarter">客戶印鑑搜尋(依客戶ID與季度)</param>
        /// <returns></returns>
        ResponseViewModel InvalidSeals(CustomerSealSearchQuarter customerSealQuarter);


    }
}
