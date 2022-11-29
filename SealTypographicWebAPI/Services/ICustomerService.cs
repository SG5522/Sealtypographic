using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// 顧客資料處理的interface
    /// </summary>
    public interface ICustomerService
    {
        /// <summary>
        /// 取得顧客資料
        /// </summary>
        /// <param name="customerId">顧客ID</param>
        /// <returns></returns>
        CustomerResponse GetCustomer(string customerId);

        /// <summary>
        /// 依搜尋條件獲得顧客資料列表
        /// </summary>
        /// <param name="customerQuery">客戶分頁搜尋</param>        
        /// <returns></returns>
        CustomerResponsePage GetCustomerViewModels(CustomerSearch customerQuery);

        /// <summary>
        /// 建立顧客資料
        /// </summary>
        /// <param name="customer">基本資料</param>
        Response CreateCustomer(CustomerData customer);

        /// <summary>
        /// 更新客戶基本資料
        /// </summary>
        /// <param name="customer">基本資料</param>
        Response UpdateCustomer(CustomerData customer);

        /// <summary>
        /// 刪除客戶基本資料(變更狀態使其一般USER無法看到)
        /// </summary>
        /// <param name="customerId"></param>
        Response DeleteCustomer(string customerId);
    }
}
