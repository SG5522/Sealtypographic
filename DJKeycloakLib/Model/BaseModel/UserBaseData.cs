using System.Text.Json.Serialization;

namespace DJKeycloakLib.Model.BaseModel
{
    /// <summary>
    /// KeycloakUser 基本資料
    /// </summary>
    public abstract class UserBaseData
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
