using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.Customer;

namespace SealTypographicWebAPI.Services.Accountant
{
    /// <summary>
    /// 會計師
    /// </summary>
    public interface IAccountantGroupService
    {

        /// <summary>
        /// 取得會計師資料
        /// </summary>
        /// <param name="accountantId"></param>
        /// <returns></returns>
        AccountantResponse GetAccountant(string accountantId);

        /// <summary>
        /// 依搜尋條件獲得會計資料列表
        /// </summary>
        /// <param name="idOrNmaeOrGroupsName">會計師ID或名字或是群組名稱</param>
        /// <param name="thisPage">現在頁次</param>
        /// <param name="pageSize">單頁資料量</param>     
        /// <returns></returns>
        AccountantsResponse GetAccountantViewModels(string idOrNmaeOrGroupsName, int thisPage, int pageSize);

        /// <summary>
        /// 建立會計師群組資料
        /// </summary>
        /// <param name="accountantGroupData">群組資料</param>        
        Response CreateAccountantGroup(AccountantGroupData accountantGroupData);


        /// <summary>
        /// 更新會計師群組資料
        /// </summary>
        /// <param name="accountantGroupData">群組資料</param>
        Response UpdateAccountantGroup(AccountantGroupData accountantGroupData);

        /// <summary>
        /// 刪除客戶基本資料(變更狀態使其一般USER無法看到)
        /// </summary>
        /// <param name="accountantGroupDataId"></param>
        Response DeleteAccountantGroup(string accountantGroupDataId);
    }

}
