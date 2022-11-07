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
        List<CustomerSeal> GetcustomerSeals(string customerID, string quarter);

        /// <summary>
        /// 取得顧客基本資料
        /// </summary>
        /// <param name="customerID">顧客ID</param>
        /// <returns></returns>
        CustomerDataAddID GetCustomerData(string customerID);

        /// <summary>
        /// 依搜尋條件獲得顧客資料列表
        /// </summary>        
        /// <param name="customerIDOrName">顧客ID或名字</param>
        /// <returns></returns>
        List<CustomerListData> GetCustomerList(string customerIDOrName);

    }
}
