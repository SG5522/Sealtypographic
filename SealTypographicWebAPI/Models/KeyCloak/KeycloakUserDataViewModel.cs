using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models.Keycloak
{
    /// <summary>
    /// 顯示KeycloakUser 資料
    /// </summary>
    public class KeycloakUserDataViewModel : KeycloakUserBaseData
    {
        /// <summary>
        /// Keycloak UserId(UUID)
        /// </summary>
        [JsonPropertyName("id")]
        public string UserId { get; set; }
    }
}
