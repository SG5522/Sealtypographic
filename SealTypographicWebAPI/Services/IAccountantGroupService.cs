using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.AccountantGroup;
using SealTypographicWebAPI.Models.Customer;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// 會計師
    /// </summary>
    public interface IAccountantGroupService
    {
        /// <summary>
        /// 取得會計師群組所有資料
        /// </summary>
        /// <returns></returns>
        AccountantGroupList GetAccountantGroupList();

        /// <summary>
        /// 取得會計師資料
        /// </summary>
        /// <param name="accountantGroupId">群組ID</param>
        /// <returns></returns>
        AccountantGroupResponse GetAccountantGroupData(int accountantGroupId);

        /// <summary>
        /// 依搜尋條件獲得會計資料列表
        /// </summary>
        /// <param name="accountantGroupQueryPage">會計師群組分頁搜尋</param> 
        /// <returns></returns>
        AccountantGroupResponses GetAccountantGroups(AccountantGroupSearch accountantGroupQueryPage);

        /// <summary>
        /// 建立會計師群組資料
        /// </summary>
        /// <param name="accountantGroupForm">群組資料</param>        
        ResponseViewModel CreateAccountantGroup(AccountantGroupForm accountantGroupForm);

        /// <summary>
        /// 更新會計師群組資料
        /// </summary>
        /// <param name="accountantGroupFormUpdate">群組資料</param>
        ResponseViewModel UpdateAccountantGroup(AccountantGroupFormUpdate accountantGroupFormUpdate);

        /// <summary>
        /// 刪除群組(將該群組的所有人員先轉移到無群組在進行群組刪除)
        /// </summary>
        /// <param name="accountantGroupDataId">會計師群組ID</param>
        ResponseViewModel DeleteAccountantGroup(int accountantGroupDataId);
    }

}
