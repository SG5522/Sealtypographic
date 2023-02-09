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
        /// 取得群組所有資料
        /// </summary>
        /// <returns></returns>
        AccountantGroupList GetAll();

        /// <summary>
        /// 取得群組資料(單筆)
        /// </summary>
        /// <param name="accountantGroupId">群組ID</param>
        /// <returns></returns>
        AccountantGroupResponse GetData(int accountantGroupId);

        /// <summary>
        /// 依搜尋條件取得群組列表(分頁)
        /// </summary>
        /// <param name="accountantGroupSearch">群組搜尋條件(分頁)</param> 
        /// <returns></returns>
        AccountantGroupPaginateViewModel GetPaginate(AccountantGroupSearch accountantGroupSearch);

        /// <summary>
        /// 新增群組
        /// </summary>
        /// <param name="accountantGroupForm">群組資料</param>      
        /// <returns></returns>
        ResponseViewModel New(AccountantGroupForm accountantGroupForm);

        /// <summary>
        /// 更新群組資料
        /// </summary>
        /// <param name="accountantGroupFormUpdate">群組資料(含Id)</param>
        /// <returns></returns>
        ResponseViewModel Update(AccountantGroupUpdateForm accountantGroupFormUpdate);

        /// <summary>
        /// 刪除群組(將該群組的所有人員先轉移到無群組在進行群組刪除)
        /// </summary>
        /// <param name="accountantGroupId">群組Id</param>
        /// <returns></returns>
        ResponseViewModel Delete(int accountantGroupId);
    }

}
