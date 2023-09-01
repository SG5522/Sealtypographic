using SealTypographicWebAPI.Models.Keycloak;
using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models.KeyCloak
{
    /// <summary>
    /// KeyCloak的使用者註冊資料
    /// </summary>
    public class KeycloakUserDataForm : KeycloakUserBaseData
    {
        /// <summary>
        /// 密碼資料
        /// </summary>
        [JsonPropertyName("credentials")]
        public List<KeycloakCredentials> Credentials { get; set; }
    }
}
