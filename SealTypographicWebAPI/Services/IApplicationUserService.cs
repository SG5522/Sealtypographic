using DJKeycloakAPI.Models.Users;
using DJKeycloakLib.Models.BaseModel;
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

        /// <summary>
        /// 新增User(db上的新增)
        /// </summary>
        /// <param name="newUserForm"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<ResponseModel> AddUser(NewUserForm newUserForm, string userName = "admin");

    }
}
