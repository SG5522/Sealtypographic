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
        CustomerSealQuarters GetCustomerSealQuarters(string customerID);


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
        Response CreateCustomerSeals(List<CustomerSeal> customerSeals);

        /// <summary>
        /// 修改印鑑組
        /// </summary>
        /// <param name="customerSeals">印鑑資料</param>
        /// <returns></returns>
        Response UpdateCustomerSeals(List<CustomerSealPostData> customerSeals);

        /// <summary>
        /// 刪除印鑑組
        /// </summary>
        /// <param name="customerSeals">印鑑資料</param>
        /// <returns></returns>
        Response DeleteCustomerSeals(List<CustomerSeal> customerSeals);

    }
}
