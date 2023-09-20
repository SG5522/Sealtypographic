using System.Text.Json.Serialization;
using DJKeycloakLib.Model.BaseModel;

namespace DJKeycloakLib.Model
{
    /// <summary>
    /// 顯示KeycloakUser 資料
    /// </summary>
    public class UserDetailViewModel : UserBaseData
    {
        /// <summary>
        /// Keycloak UserId(UUID)
        /// </summary>
        [JsonPropertyName("id")]
        public string UserId { get; set; }
    }
}
