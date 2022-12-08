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
        CustomerDetailViewModel GetCustomerDetailViewModel(int customerId);

        /// <summary>
        /// 依搜尋條件獲得顧客資料列表
        /// </summary>
        /// <param name="customerQuery">客戶分頁搜尋</param>        
        /// <returns></returns>
        CustomerPaginateViewModel GetCustomerPaginatesViewModel(CustomerSearch customerQuery);

        /// <summary>
        /// 建立顧客資料
        /// </summary>
        /// <param name="customer">基本資料</param>
        ResponseViewModel CreateCustomer(CustomerForm customer);

        /// <summary>
        /// 更新客戶基本資料
        /// </summary>
        /// <param name="customer">基本資料</param>
        ResponseViewModel UpdateCustomer(CustomerFormUpdate customer);

        /// <summary>
        /// 刪除客戶基本資料(變更狀態使其一般USER無法看到)
        /// </summary>
        /// <param name="customerId"></param>
        ResponseViewModel DeleteCustomer(int customerId);
    }
}
