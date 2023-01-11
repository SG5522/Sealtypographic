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
        /// 取得會計師資料
        /// </summary>
        /// <param name="accountantId"></param>
        /// <returns></returns>
        AccountantDetailResponse GetDetail(int accountantId);

        /// <summary>
        /// 依搜尋條件獲得會計資料列表
        /// </summary>
        /// <param name="accountantQueryPage">會計師分頁搜尋</param>  
        /// <returns></returns>
        AccountantPaginateViewModel GetPaginate(AccountantSearch accountantQueryPage);

        /// <summary>
        /// 建立會計師資料
        /// </summary>
        /// <param name="accountantBaseData">基本資料</param>        
        AccountantCreateResponse Create(AccountantForm accountantBaseData);


        /// <summary>
        /// 更新客戶基本資料
        /// </summary>
        /// <param name="accountantBaseData">基本資料</param>
        ResponseViewModel Update(AccountantFormUpdate accountantBaseData);

        /// <summary>
        /// 刪除客戶基本資料(變更狀態使其一般USER無法看到)
        /// </summary>
        /// <param name="accountantId"></param>
        ResponseViewModel Delete(int accountantId);
    }

}
