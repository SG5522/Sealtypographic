using System.Data;

namespace SealTypographicWebAPI.Consts
{
    /// <summary>
    /// Keycloak RestAPI Url
    /// </summary>
    public class KeycloakAdminUrlConsts
    {
        /// <summary>
        /// 從UserID搜尋User資料
        /// </summary>
        public const string UsersQueryWithId = "users/{id}/";

        /// <summary>
        /// 從UserName搜尋User資料
        /// </summary>
        public const string UsersQueryWithUserName = "users";

        /// <summary>
        /// 從該UserId取得Group資料
        /// </summary>
        public const string UserGroup = "users/{id}/groups";

        /// <summary>
        /// Users Count數
        /// </summary>
        public const string UsersCount = "users/count";

        /// <summary>
        /// 取得Role資料
        /// </summary>        
        public const string Role = "clients/{id}/roles";

        /// <summary>
        /// 取得GroupRole資料
        /// </summary>        
        public const string GroupRoleMapping = "groups/{id}/role-mappings/clients/{client}";

        /// <summary>
        /// 重設密碼
        /// </summary>
        public const string ResetPassword = "users/{id}/reset-password";
    }
}
