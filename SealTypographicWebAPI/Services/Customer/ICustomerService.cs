using SealTypographicWebAPI.Models;

namespace SealTypographicWebAPI.Services.Customer
{   
    /// <summary>
    /// 顧客資料處理的interface
    /// </summary>
    public interface ICustomerService
    {
        /// <summary>
        /// 取得顧客印鑑組
        /// </summary>
        /// <param name="customerID">顧客ID</param>
        /// <param name="quarter">季度</param>
        /// <returns></returns>
        CustomerSeals GetCustomerSeals(string customerID, string quarter);

        /// <summary>
        /// 取得顧客基本資料
        /// </summary>
        /// <param name="customerID">顧客ID</param>
        /// <returns></returns>
        Models.Customer GetCustomer(string customerID);

        /// <summary>
        /// 依搜尋條件獲得顧客資料列表
        /// </summary>        
        /// <param name="customerIDOrName">顧客ID或名字</param>
        /// <returns></returns>
        CustomerResponseViewModel GetCustomerViewModels(string customerIDOrName);

        /// <summary>
        /// 建立顧客資料
        /// </summary>
        /// <param name="customer"></param>
        void CreateCustomer(Models.Customer customer);

    }
}
