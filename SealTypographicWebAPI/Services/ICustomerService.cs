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
        /// <returns></returns>
        CustomerDetailViewModel GetDetail(int customerId);

        /// <summary>
        /// 取得簡化的客戶資料
        /// </summary>
        /// <param name="customerId">客戶Id</param>
        /// <returns></returns>
        CustomerSummaryResponse GetSummary(int customerId);

        /// <summary>
        /// 取得客戶資料列表(分頁)
        /// </summary>
        /// <param name="customerSearch">客戶分頁搜尋</param>        
        /// <returns></returns>
        CustomerPaginateViewModel GetPaginate(CustomerSearch customerSearch);

        /// <summary>
        /// 取得客戶資料列表(簡化資料的分頁)
        /// </summary>
        /// <param name="customerSearch">客戶分頁搜尋</param>
        /// <returns></returns>
        CustomerPaginateShort GetPaginateShort(CustomerSearch customerSearch);

        /// <summary>
        /// 新增客戶基本資料
        /// </summary>
        /// <param name="customerForm">基本資料</param>
        CreateCustomerResponse New(CustomerForm customerForm);

        /// <summary>
        /// 更新基本資料
        /// </summary>
        /// <param name="customerUpdateForm">基本資料</param>
        ResponseViewModel Update(CustomerUpdateForm customerUpdateForm);

        /// <summary>
        /// 刪除基本資料，
        /// 此刪除為更動狀態使其一般使用者看不到資料，
        /// 而不是真正的刪除。
        /// </summary>
        /// <param name="customerId"></param>
        ResponseViewModel Delete(int customerId);
    }
}
