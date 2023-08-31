using SealTypographicWebAPI.Models.KeyCloak;
using SealTypographicWebAPI.Models;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// KeyCloak Admin帳號管理
    /// </summary>
    public interface IKeycloakAdminService
    {
        /// <summary>
        /// 取得使用者資料
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<string> GetUserData(string username);

        /// <summary>
        /// 取得Roles
        /// </summary>        
        /// <returns></returns>
        Task<KeycloakClientRolesResponse> GetRoles();

        /// <summary>
        /// 新增使用者
        /// </summary>
        /// <param name="keyCloakUserData"></param>
        /// <returns></returns>
        Task<ResponseViewModel> New(KeycloakUserData keyCloakUserData);

        /// <summary>
        /// 重設密碼
        /// </summary>
        /// <param name="userId">userId</param>
        /// <param name="keycloakCredentials">密碼資料</param>
        /// <returns></returns>
        Task<ResponseViewModel> ResetPassword(string userId, KeycloakCredentials keycloakCredentials);
    }
}
