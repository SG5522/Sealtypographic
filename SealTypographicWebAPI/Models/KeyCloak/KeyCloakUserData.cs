using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models.KeyCloak
{
    /// <summary>
    /// KeyCloak的使用者註冊資料
    /// </summary>
    public class KeycloakUserData
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }

        /// <summary>
        /// 帳號
        /// </summary>
        [JsonPropertyName("username")]
        public string UserName { get; set; }

        /// <summary>
        /// 密碼資料
        /// </summary>
        [JsonPropertyName("credentials")]
        public List<KeycloakCredentials> Credentials { get; set; }
    }
}
