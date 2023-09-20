using System.Text.Json.Serialization;
using DJKeycloakLib.Model.BaseModel;

namespace DJKeycloakLib.Model
{
    /// <summary>
    /// KeyCloak的使用者註冊資料
    /// </summary>
    public class UserDataForm : UserBaseData
    {
        /// <summary>
        /// 密碼資料
        /// </summary>
        [JsonPropertyName("credentials")]
        public List<Credentials> Credentials { get; set; }
    }
}
