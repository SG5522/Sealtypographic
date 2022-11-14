using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;

namespace SealTypographicWebAPI.Services.Customer
{
    /// <summary>
    /// 顧客印鑑組Interface
    /// </summary>
    public interface ICustomerSealService
    {
        /// <summary>
        /// 取得顧客印鑑組
        /// </summary>
        /// <param name="customerID">顧客ID</param>
        /// <param name="quarter">季度</param>
        /// <returns></returns>
        CustomerSeals GetCustomerSeals(string customerID, string quarter);

        /// <summary>
        /// 新增印鑑組
        /// </summary>
        /// <param name="customerSeals"></param>
        /// <returns></returns>
        Response CreateCustomerSeals(List<CustomerSeal> customerSeals);
    }
}
