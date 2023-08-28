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
        public const string Users = "users";

        /// <summary>
        /// Users Count數
        /// </summary>
        public const string UsersCount = "users/count";

        /// <summary>
        /// Role資料
        /// </summary>
        //public const string Role = "clients/79c9ec63-c06a-4386-888a-1e7b71478f05/roles";
        public const string Role = "clients/{id}/roles";
    }
}
