using System.Text.Json.Serialization;

namespace SealAPIWrap.Models
{
    /// <summary>
    /// 圖像處理Api傳入資料
    /// </summary>
    public class ImageProcessForm : ImageProcessRequest
    {
        /// <summary>
        /// Config資料夾
        /// </summary>
        [JsonPropertyName("config")]
        public string Config { get; set; }
    }
}
