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
        /// 取得會計師基本資料
        /// </summary>
        /// <param name="accountantId"></param>
        /// <returns></returns>
        AccountantDetailResponse GetDetail(int accountantId);

        /// <summary>
        /// 依搜尋條件獲得會計師資料列表
        /// </summary>
        /// <param name="accountantSearch">搜尋條件</param>  
        /// <returns></returns>
        AccountantPaginateViewModel GetPaginate(AccountantSearch accountantSearch);

        /// <summary>
        /// 新增會計師基本資料
        /// </summary>
        /// <param name="accountantForm">會計師基本資料</param>        
        AccountantCreateResponse New(AccountantForm accountantForm);


        /// <summary>
        /// 更新客戶基本資料
        /// </summary>
        /// <param name="accountantBaseData">基本資料</param>
        ResponseViewModel Update(AccountantUpdateForm accountantBaseData);

        /// <summary>
        /// 刪除基本資料，
        /// 此刪除為更動狀態使其一般使用者看不到資料，
        /// 而不是真正的刪除。
        /// </summary>
        /// <param name="accountantId">會計師ID</param>
        ResponseViewModel Delete(int accountantId);
    }

}
