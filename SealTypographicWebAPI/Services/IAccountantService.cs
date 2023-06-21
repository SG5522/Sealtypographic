using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.Customer;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// 會計師資料管理
    /// </summary>
    public interface IAccountantService
    {

        /// <summary>
        /// 取得資料
        /// </summary>
        /// <param name="accountantId">會計師Id</param>
        /// <returns></returns>
        AccountantDetailResponse GetDetail(int accountantId);

        /// <summary>
        /// 依搜尋條件獲得資料列表
        /// </summary>
        /// <param name="accountantSearch">搜尋條件</param>
        /// <param name="isTypographicUse">是否給排版使用</param>  
        /// <returns></returns>
        AccountantPaginateViewModel GetPaginate(AccountantSearch accountantSearch, bool isTypographicUse);

        /// <summary>
        /// 新增資料
        /// </summary>
        /// <param name="accountantForm">會計師資料</param>        
        AccountantCreateResponse New(AccountantForm accountantForm);


        /// <summary>
        /// 更新資料
        /// </summary>
        /// <param name="accountantBaseData">會計師資料(含Id)</param>
        ResponseViewModel Update(AccountantUpdateForm accountantBaseData);

        /// <summary>
        /// 刪除資料，
        /// 此刪除為更動狀態使其一般使用者看不到資料，
        /// 而不是真正的刪除。
        /// </summary>
        /// <param name="accountantId">會計師ID</param>
        ResponseViewModel Delete(int accountantId);
    }

}
