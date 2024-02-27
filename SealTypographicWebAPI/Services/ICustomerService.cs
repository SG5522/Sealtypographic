using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Models.CustomerSealReview;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// 管理客戶資料
    /// </summary>
    public interface ICustomerService
    {
        /// <summary>
        /// 取得客戶詳細基本資料
        /// </summary>
        /// <param name="customerId">客戶ID</param>
        /// <param name="userId">登入的使用者Id</param>
        /// <returns></returns>
        Task<CustomerDetailViewModel> GetDetail(int customerId, int userId = 1);

        /// <summary>
        /// 取得客戶資料列表(分頁)
        /// </summary>
        /// <param name="customerSearch">客戶分頁搜尋</param>
        /// <param name="userId">登入的使用者Id</param>
        /// <returns></returns>
        Task<CustomerPaginateSummary> GetPaginate(CustomerSearch customerSearch, int userId = 1);


        /// <summary>
        /// 新增客戶基本資料
        /// </summary>
        /// <param name="customerForm">基本資料</param>
        /// <param name="userId">登入的使用者Id</param>
        CreateCustomerResponse New(CustomerForm customerForm, int userId = 1);

        /// <summary>
        /// 更新基本資料
        /// </summary>
        /// <param name="customerUpdateForm">基本資料</param>
        /// <param name="userId">登入的使用者Id</param>
        ResponseViewModel Update(CustomerUpdateForm customerUpdateForm, int userId = 1);

        /// <summary>
        /// 刪除基本資料，
        /// 此刪除為更動狀態使其一般使用者看不到資料，
        /// 而不是真正的刪除。
        /// </summary>
        /// <param name="customerId">客戶Id</param>
        /// <param name="userId">登入的使用者Id</param>
        ResponseViewModel Delete(int customerId, int userId = 1);
    }
}
