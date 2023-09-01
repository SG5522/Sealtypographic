using SealTypographicWebAPI.Models.KeyCloak;
using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models.Keycloak
{
    /// <summary>
    /// KeycloakUser 基本資料
    /// </summary>
    public abstract class KeycloakUserBaseData
    {
        /// <summary>
        /// 是否啟用
        /// </summary>
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }

        /// <summary>
        /// 帳號
        /// </summary>
        [JsonPropertyName("username")]
        public string UserName { get; set; }
    }
}
