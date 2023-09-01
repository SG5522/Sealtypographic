using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models.Keycloak
{
    /// <summary>
    /// Keycloak 使用者群組
    /// </summary>
    public class KeycloakUserGroup
    {
        /// <summary>
        /// Keycloak GroupId(UUID)
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Keycloak GroupName
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Keycloak GroupPath
        /// </summary>
        [JsonPropertyName("path")]
        public string Path { get; set; }
    }
}
