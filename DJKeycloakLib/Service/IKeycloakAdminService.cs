using DJKeycloakLib.Model;
using DJKeycloakLib.Model.BaseModel;

namespace DJKeycloakLib.Service
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
        Task<UserDetailResponse> GetUserData(string username);

        /// <summary>
        /// 取得Roles
        /// </summary>        
        /// <returns></returns>
        Task<ClientRolesResponse> GetRoles();

        /// <summary>
        /// 取得群組的角色權限關聯
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<string> GroupRoleMappings(string groupId);

        /// <summary>
        /// 取得KeycloakUserData分頁
        /// </summary>
        /// <param name="userSearch ">Keycloak User搜尋條件</param>
        /// <returns></returns>
        Task<UserDataPaginate> GetUserDataPaginate(UserSearch userSearch);

        /// <summary>
        /// 新增使用者
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        Task<ResponseBaseModel> New(UserDataForm userData);

        /// <summary>
        /// 重設密碼
        /// </summary>
        /// <param name="userName">userId</param>
        /// <param name="credentials">密碼資料</param>
        /// <returns></returns>
        Task<ResponseBaseModel> ResetPassword(string userName, Credentials credentials);
    }
}
