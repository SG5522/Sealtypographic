using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.AccountantGroup;

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
        /// <param name="userId">登入的使用者Id</param>
        /// <returns></returns>
        Task<AccountantGroupResponse> GetData(int accountantGroupId, int userId = 1);

        /// <summary>
        /// 依搜尋條件取得群組列表(分頁)
        /// </summary>
        /// <param name="accountantGroupSearch">群組搜尋條件(分頁)</param>
        /// <param name="userId">登入的使用者Id</param> 
        /// <returns></returns>
        Task<AccountantGroupPaginateViewModel> GetPaginate(AccountantGroupSearch accountantGroupSearch, int userId = 1);

        /// <summary>
        /// 新增群組
        /// </summary>
        /// <param name="accountantGroupForm">群組資料</param>
        /// <param name="userId">登入的使用者Id</param>
        /// <returns></returns>
        Task<ResponseViewModel> New(AccountantGroupForm accountantGroupForm, int userId = 1);

        /// <summary>
        /// 更新群組資料
        /// </summary>
        /// <param name="accountantGroupFormUpdate">群組資料(含Id)</param>
        /// <param name="userId">登入的使用者Id</param>
        /// <returns></returns>
        Task<ResponseViewModel> Update(AccountantGroupUpdateForm accountantGroupFormUpdate, int userId = 1);

        /// <summary>
        /// 刪除群組(將該群組的所有人員先轉移到無群組在進行群組刪除)
        /// </summary>
        /// <param name="accountantGroupId">群組Id</param>
        /// <param name="userId">登入的使用者Id</param>
        /// <returns></returns>
        Task<ResponseViewModel> Delete(int accountantGroupId, int userId = 1);
    }
}
