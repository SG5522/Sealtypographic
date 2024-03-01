using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// 登入使用者基本資訊
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// Keycloak UserId
        /// </summary>
        [JsonIgnore]
        public string KeycloakUserId { get; set; }

        /// <summary>
        /// 使用者Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 使用者名稱(帳號)
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 使用者暱稱
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// 使用者暱稱
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// 角色權限
        /// </summary>
        public List<string> Roles { get; private set; } = new List<string>(); // 可寫入，但只能在類內部設置值

        /// <summary>
        /// 提供方法來設置 Roles 屬性的值
        /// </summary>
        /// <param name="roles"></param>
        public void SetRoles(List<string> roles)
        {
            Roles = roles;
        }
    }
}
