using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Models.CustomerSealReview;

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
        CustomerDetailViewModel GetDetail(int customerId);

        /// <summary>
        /// 依搜尋條件獲得顧客資料列表
        /// </summary>
        /// <param name="customerSearch">客戶分頁搜尋</param>        
        /// <returns></returns>
        CustomerPaginateViewModel GetPaginate(CustomerSearch customerSearch);

        /// <summary>
        /// 建立顧客資料
        /// </summary>
        /// <param name="customerForm">基本資料</param>
        CreateCustomerResponse Create(CustomerForm customerForm);

        /// <summary>
        /// 更新客戶基本資料
        /// </summary>
        /// <param name="customerUpdateForm">基本資料</param>
        ResponseViewModel Update(CustomerUpdateForm customerUpdateForm);

        /// <summary>
        /// 刪除客戶基本資料(變更狀態使其一般USER無法看到)
        /// </summary>
        /// <param name="customerId"></param>
        ResponseViewModel Delete(int customerId);
    }
}
