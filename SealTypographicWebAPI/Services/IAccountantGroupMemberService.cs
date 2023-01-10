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
        /// 取得此群組底下的會計師
        /// </summary>
        /// <param name="accountantGroupMemberSearch">會計群組搜尋條件(分頁)</param>
        /// <returns></returns>
        AccountantGroupMembers GetAccountantGroupMembers(AccountantGroupMemberSearch accountantGroupMemberSearch);

        /// <summary>
        /// 取得非此群組的成員
        /// </summary>
        /// <param name="notThisGroupMemberSearch">搜尋條件</param>
        NotThisGroupMember GetNotThisGroupMember(NotThisGroupMemberSearch notThisGroupMemberSearch);

        /// <summary>
        /// 變更會計師群組(單個)
        /// </summary>
        /// <param name="accountantGroupChangeForm">會計群組變更資料</param>
        /// <returns></returns>
        ResponseViewModel UpdateAccountantGroup(AccountantGroupChangeForm accountantGroupChangeForm);

        /// <summary>
        /// 變更多個會計師的群組
        /// </summary>
        /// <param name="accountantGroupMemberForm"></param>
        /// <returns></returns>
        ResponseViewModel ChangeNotTheGroupMember(AccountantGroupMemberForm accountantGroupMemberForm);
    }
}
