using System.Text.Json.Serialization;

namespace SealAPIWrap.Models
{
    /// <summary>
    /// 驗印傳入細項資料
    /// </summary>
    public class SealIdentifyItem
    {
        /// <summary>
        /// 編號
        /// </summary>
        [JsonPropertyName("index")]
        public int Index { get; set; }

        /// <summary>
        /// 圖像Base64
        /// </summary>
        [JsonPropertyName("stampBase64")]
        public string StampBase64 { get; set; }

        /// <summary>
        /// 顏色
        /// </summary>
        [JsonPropertyName("color")]
        public Color Color { get; set; }

        /// <summary>
        /// 忽略區域
        /// 左上角X-左上角Y_右下角X-右下角Y|左上角X-左上角Y_右下角X-右下角Y
        /// </summary>
        /// <example>
        /// 50-50_100-100
        /// </example>
        [JsonPropertyName("em")]
        public string Em { get; set; }
    }
}
