using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Keycloak
{
    /// <summary>
    /// Keycloak User搜尋條件
    /// </summary>
    public class KeycloakUserSearch : PaginateSearch
    {
        /// <summary>
        /// 使用者帳號
        /// </summary>
        public string? UserName { get; set; }
    }
}
