using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SealAPIWrap.Models
{
    /// <summary>
    /// 驗印Api傳入資料
    /// </summary>
    public class SealIdentifyForm : SealIdentifyRequest
    {
        /// <summary>
        /// Config資料夾
        /// </summary>
        [JsonPropertyName("config")]
        public string Config { get; set; }
    }
}
