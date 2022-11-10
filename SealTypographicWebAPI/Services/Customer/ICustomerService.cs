using SealTypographicWebAPI.Models;

namespace SealTypographicWebAPI.Services.Customer
{   
    /// <summary>
    /// 顧客資料處理的interface
    /// </summary>
    public interface ICustomerService
    {
        /// <summary>
        /// 取得顧客基本資料
        /// </summary>
        /// <param name="customerId">顧客ID</param>
        /// <returns></returns>
        CustomerResponse GetCustomer(string customerId);

        /// <summary>
        /// 依搜尋條件獲得顧客資料列表
        /// </summary>        
        /// <param name="customerIDOrName">顧客ID或名字</param>
        /// <param name="thispage">現在頁次</param>
        /// <returns></returns>
        CustomerResponseViewModel GetCustomerViewModels(string customerIDOrName,int thispage);

        /// <summary>
        /// 建立顧客資料
        /// </summary>
        /// <param name="customer"></param>
        void CreateCustomer(CustomerBaseData customer);

    }
}
