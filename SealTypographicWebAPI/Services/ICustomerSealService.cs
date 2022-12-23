using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// 顧客印鑑組Interface
    /// </summary>
    public interface ICustomerSealService
    {
        /// <summary>
        /// 取得顧客印鑑季度表
        /// </summary>
        /// <param name="customerID">顧客ID</param>        
        /// <returns></returns>
        CustomerSealQuarters GetCustomerSealQuarters(int customerID);


        /// <summary>
        /// 取得顧客印鑑
        /// </summary>
        /// <param name="customerSealQuarter"></param>
        /// <returns></returns>
        CustomerSealViewModels GetCustomerSealViewModels(CustomerSealQuarter customerSealQuarter);

        /// <summary>
        /// 新增印鑑組
        /// </summary>
        /// <param name="customerSeals">印鑑資料</param>
        /// <returns></returns>
        ResponseViewModel CreateCustomerSeals(List<CustomerSealForm> customerSeals);

        /// <summary>
        /// 異動客戶印鑑
        /// </summary>
        /// <param name="customerSealUpdate">刪除修改新增的list</param>
        /// <returns></returns>        
        List<ResponseViewModel> UpdateCustomerSeals(CustomerSealUpdate customerSealUpdate);

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

        /// <summary>
        /// 刪除印鑑 (隱藏)
        /// </summary>
        /// <param name="customerSealId"></param>
        /// <returns></returns>
        ResponseViewModel DeleteCustomerSeal(int customerSealId);

    }
}
