using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models.KeyCloak
{
    /// <summary>
    /// KeyCloak的使用者密碼相關
    /// </summary>
    public class KeycloakCredentials
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// 使用者密碼
        /// </summary>
        [JsonPropertyName("value")]
        public string Value { get; set; }

        /// <summary>
        /// 是否重設密碼
        /// </summary>
        [JsonPropertyName("temporary")]
        public bool Temporary { get; set; }
    }
}
