using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.AccountantGroupMember;
using SealTypographicWebAPI.Models.Customer;
using System.Security.Claims;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// 會計師群組成員管理
    /// </summary>
    public interface IApplicationUserService
    {
        /// <summary>
        /// 取得登入成功的使用者資訊
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        UserInfo GetUserInfo(ClaimsPrincipal claims);

    }
}
