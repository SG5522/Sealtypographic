using System.Text.Json.Serialization;

namespace SealAPIWrap.Models
{
    /// <summary>
    /// 驗印結果輔助顯示傳入資料
    /// </summary>
    public class SealShowForm : SealShowRequest
    {
        /// <summary>
        /// Config資料夾
        /// </summary>
        [JsonPropertyName("config")]
        public string Config { get; set; }
    }
}
