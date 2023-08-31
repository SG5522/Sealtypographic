namespace SealTypographicWebAPI.Consts
{
    /// <summary>
    /// Keycloak取
    /// </summary>
    public class KeycloakAdminUrlConsts
    {
        /// <summary>
        /// Users管理相關
        /// </summary>
        public const string Users = "users/{id}/";

        /// <summary>
        /// Users Count數
        /// </summary>
        public const string UsersCount = "users/count";

        /// <summary>
        /// Role資料
        /// </summary>        
        public const string Role = "clients/{id}/roles";

        /// <summary>
        /// 重設密碼
        /// </summary>
        public const string ResetPassword = "users/{id}/reset-password";
    }
}
