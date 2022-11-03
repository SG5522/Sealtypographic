using SealTypographicWebAPI.Models;

namespace SealTypographicWebAPI.Service.Customer
{   
    /// <summary>
    /// 顧客資料處理的interface
    /// </summary>
    public interface ICustomer
    {
        /// <summary>
        /// 取得顧客印鑑組
        /// </summary>
        /// <param name="customerID">顧客ID</param>
        /// <param name="quarter">季度</param>
        /// <returns></returns>
        List<CustomerSeal> GetcustomerSeals(int customerID, string quarter);

        /// <summary>
        /// 取得顧客基本資料
        /// </summary>
        /// <param name="customerID">顧客ID</param>
        /// <returns></returns>
        CustomerDataAddID GetCustomerData(int customerID);
    }
}
