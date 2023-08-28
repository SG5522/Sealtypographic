using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models.KeyCloak
{
    /// <summary>
    /// 該Role的名稱與說明
    /// </summary>
    public class KeyCloakClientRole
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
        /// 別名(暫定用中文)
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// 說明
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
