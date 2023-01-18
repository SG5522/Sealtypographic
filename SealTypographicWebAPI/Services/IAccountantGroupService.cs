using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.AccountantGroup;
using SealTypographicWebAPI.Models.Customer;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// 管理會計師群組
    /// </summary>
    public interface IAccountantGroupService
    {
        /// <summary>
        /// 取得會計師群組所有資料
        /// </summary>
        /// <returns></returns>
        AccountantGroupList GetAll();

        /// <summary>
        /// 取得會計師資料
        /// </summary>
        /// <param name="accountantGroupId">群組ID</param>
        /// <returns></returns>
        AccountantGroupResponse GetData(int accountantGroupId);

        /// <summary>
        /// 依搜尋條件獲得會計師資料列表(分頁)
        /// </summary>
        /// <param name="accountantGroupSearch">會計師群組搜尋條件(分頁)</param> 
        /// <returns></returns>
        AccountantGroupResponses GetPaginate(AccountantGroupSearch accountantGroupSearch);

        /// <summary>
        /// 新增會計師群組
        /// </summary>
        /// <param name="accountantGroupForm">群組資料</param>        
        ResponseViewModel New(AccountantGroupForm accountantGroupForm);

        /// <summary>
        /// 更新會計師群組資料
        /// </summary>
        /// <param name="accountantGroupFormUpdate">群組資料</param>
        ResponseViewModel Update(AccountantGroupFormUpdate accountantGroupFormUpdate);

        /// <summary>
        /// 刪除群組(將該群組的所有人員先轉移到無群組在進行群組刪除)
        /// </summary>
        /// <param name="accountantGroupDataId">會計師群組ID</param>
        ResponseViewModel Delete(int accountantGroupDataId);
    }

}
