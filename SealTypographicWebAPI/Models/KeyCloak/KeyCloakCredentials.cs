using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models.KeyCloak
{
    /// <summary>
    /// KeyCloak的使用者密碼相關
    /// </summary>
    public class KeycloakCredentials
    {
        /// <summary>
        /// 設定Credential類別
        /// </summary>
        /// <example>password</example>
        [JsonPropertyName("type")]
        [JsonIgnore]
        public string Type = "password";

        /// <summary>
        /// 使用者密碼
        /// </summary>
        /// <example>12345678</example>
        [JsonPropertyName("value")]
        public string Value { get; set; }

        /// <summary>
        /// 是否重設密碼
        /// </summary>
        /// <example>true</example>
        [JsonPropertyName("temporary")]
        public bool Temporary { get; set; }
    }
}
