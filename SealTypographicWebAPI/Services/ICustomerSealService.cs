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
        /// 取得顧客印鑑季度表
        /// </summary>
        /// <param name="customerID">顧客ID</param>        
        /// <returns></returns>
        CustomerSealQuarterViews GetQuarter(int customerID);


        /// <summary>
        /// 取得顧客印鑑
        /// </summary>
        /// <param name="customerSealQuarter"></param>
        /// <returns></returns>
        CustomerSealViewModels GetSeal(CustomerSealQuarter customerSealQuarter);

        /// <summary>
        /// 新增印鑑組
        /// </summary>
        /// <param name="customerSealForms">印鑑資料</param>
        /// <returns></returns>
        ResponseViewModel Create(CustomerSealForm customerSealForms);

        /// <summary>
        /// 異動客戶印鑑
        /// </summary>
        /// <param name="customerSealUpdate">刪除修改新增的list</param>
        /// <returns></returns>        
        List<ResponseViewModel> Update(CustomerSealUpdate customerSealUpdate);

        /// <summary>
        /// 變更此季度印鑑待審
        /// </summary>
        /// <param name="customerSealQuarter">客戶Id與季度</param>
        /// <returns></returns>
        ResponseViewModel PendingCustomerSeal(CustomerSealQuarter customerSealQuarter);

        /// <summary>
        /// 變更此季度印鑑作廢
        /// </summary>
        /// <param name="customerSealQuarter">客戶Id與季度</param>
        /// <returns></returns>
        ResponseViewModel InvalidCustomerSeal(CustomerSealQuarter customerSealQuarter);


    }
}
