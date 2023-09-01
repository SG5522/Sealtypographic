using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models.Keycloak
{
    /// <summary>
    /// 角色權限關聯
    /// </summary>
    public class KeycloakRoleMapping
    {
        /// <summary>
        /// id
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// 別名(暫定用Keycloak說明替代)
        /// </summary>
        [JsonPropertyName("description")]
        public string Alias { get; set; }

        /// <summary>
        /// 說明
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
