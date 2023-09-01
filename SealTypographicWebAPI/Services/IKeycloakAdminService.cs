using SealTypographicWebAPI.Models.KeyCloak;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Keycloak;

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
        Task<KeycloakUserDetailResponse> GetUserData(string username);

        /// <summary>
        /// 取得Roles
        /// </summary>        
        /// <returns></returns>
        Task<KeycloakClientRolesResponse> GetRoles();

        /// <summary>
        /// 取得群組的角色權限關聯
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<string> GroupRoleMappings(string groupId);

        /// <summary>
        /// 新增使用者
        /// </summary>
        /// <param name="keyCloakUserData"></param>
        /// <returns></returns>
        Task<ResponseViewModel> New(KeycloakUserDataForm keyCloakUserData);

        /// <summary>
        /// 重設密碼
        /// </summary>
        /// <param name="userName">userId</param>
        /// <param name="keycloakCredentials">密碼資料</param>
        /// <returns></returns>
        Task<ResponseViewModel> ResetPassword(string userName, KeycloakCredentials keycloakCredentials);
    }
}
