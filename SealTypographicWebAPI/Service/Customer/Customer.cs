using SealTypographicWebAPI.Models;

namespace SealTypographicWebAPI.Service.Customer
{
    /// <summary>
    /// 顧客資料處理
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// 宣告顧客資料處理的interface
        /// </summary>
        public readonly ICustomer _icustomer;

        /// <summary>
        /// 注入顧客interface
        /// </summary>
        /// <param name="iCustomer"></param>
        public Customer(ICustomer iCustomer)
        {
            _icustomer = iCustomer;
        }
        /// <summary>
        /// 取得顧客印鑑組
        /// </summary>
        /// <param name="customerID">顧客ID</param>
        /// <param name="quarter">季度</param>
        /// <returns></returns>
        public List<CustomerSeal> GetcustomerSeals(int customerID, string quarter)
        {
            return _icustomer.GetcustomerSeals(customerID, quarter);
        }
        /// <summary>
        /// 取得顧客基本資料
        /// </summary>
        /// <param name="customerID">顧客ID</param>
        /// <returns></returns>
        public CustomerDataAddID GetCustomerData(int customerID)
        {
            return _icustomer.GetCustomerData(customerID);
        }
    }
}
