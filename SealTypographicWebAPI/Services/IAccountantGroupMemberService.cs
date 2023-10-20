using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.AccountantGroupMember;
using SealTypographicWebAPI.Models.Customer;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// 會計師群組成員管理
    /// </summary>
    public interface IAccountantGroupMemberService
    {
        /// <summary>
        /// 取得群組成員資料
        /// </summary>
        /// <param name="accountantGroupMemberSearch">群組成員搜尋條件(分頁)</param>
        /// <param name="isGroupMember">是否為該群組成員</param>
        /// <param name="userId">登入使用者Id</param>
        /// <returns></returns>
        AccountantGroupMembers GetMembers(AccountantGroupMemberSearch accountantGroupMemberSearch, bool isGroupMember, int userId = 0);

        /// <summary>
        /// 更新會計師群組的成員
        /// </summary>
        /// <param name="accountantGroupMemberForm">會計師群組成員資料</param>
        /// <param name="userId">登入使用者Id</param>
        /// <returns></returns>
        ResponseViewModel UpdateGroupMembers(AccountantGroupMemberForm accountantGroupMemberForm, int userId = 0);

    }
}
