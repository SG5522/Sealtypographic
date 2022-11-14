using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;

namespace SealTypographicWebAPI.Services.Customer
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
        /// <param name="customerIDOrName">顧客ID或名字</param>
        /// <param name="thispage">現在頁次</param>
        /// <param name="pageSize">單頁資料量</param>
        /// <returns></returns>
        CustomerResponsePage GetCustomerViewModels(string customerIDOrName,int thispage,int pageSize);

        /// <summary>
        /// 建立顧客資料
        /// </summary>
        /// <param name="customer">基本資料</param>
        Response CreateCustomer(CustomerBaseData customer);

        /// <summary>
        /// 更新客戶基本資料
        /// </summary>
        /// <param name="customer">基本資料</param>
        Response UpdateCustomer(CustomerBaseData customer);

        /// <summary>
        /// 刪除客戶基本資料(變更狀態使其一般USER無法看到)
        /// </summary>
        /// <param name="customerId"></param>
        Response DeleteCustomer(string customerId);
    }
}
