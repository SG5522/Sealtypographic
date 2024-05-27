using SealTypographicWebAPI.Consts;
using System.Security.Claims;
using System.Text.Json;
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
        public IList<string> Roles { get; private set; } = new List<string>(); // 可寫入，但只能在類內部設置值

        /// <summary>
        /// 提供方法來設置 Roles 屬性的值
        /// </summary>
        /// <param name="claims"></param>        
        public void SetRoles(ClaimsPrincipal claims)
        {
            string? rolesJson = claims.FindFirstValue(KeycloakRoleConsts.CLAIM_TYPE);
            if (!string.IsNullOrWhiteSpace(rolesJson))
            {
                using JsonDocument jd = JsonDocument.Parse(rolesJson);
                Roles = JsonSerializer.Deserialize<List<string>>
                        (jd.RootElement.GetProperty(KeycloakRoleConsts.RESOURCE_NAME)
                        .GetProperty("roles")
                        .GetRawText())?
                        .OrderBy(role => role).ToList() ?? new List<string>();                
            }
            else
            {
                Roles.Clear();              
            }

            //Roles = claims.Claims.Where(c => c.Type == "role").OrderBy(x => x.Value).Select(x => x.Value).ToList();
        }
    }
}
