using System.Text.Json.Serialization;

namespace SealAPIWrap.Models
{
    /// <summary>
    /// 建印Api傳入資料
    /// </summary>
    public class SealBuildForm : SealBuildRequest
    {
        /// <summary>
        /// Config資料夾
        /// </summary>
        [JsonPropertyName("config")]
        public string Config { get; set; }
    }
}
