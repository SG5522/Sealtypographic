using DJKeycloakLib.Model.BaseModel;

namespace DJKeycloakLib.Model
{
    /// <summary>
    /// Keycloak User搜尋條件
    /// </summary>
    public class UserSearch : PaginateSearch
    {
        /// <summary>
        /// 使用者帳號
        /// </summary>
        public string? UserName { get; set; }
    }
}
