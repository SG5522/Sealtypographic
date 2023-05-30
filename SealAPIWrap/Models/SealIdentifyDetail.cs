using System.Text.Json.Serialization;

namespace SealAPIWrap.Models
{
    /// <summary>
    /// 驗印細項資料
    /// </summary>
    public class SealIdentifyDetail
    {
        /// <summary>
        /// 編號
        /// </summary>
        [JsonPropertyName("index")]
        public int Index { get; set; }

        /// <summary>
        /// 驗印結果
        /// </summary>
        [JsonPropertyName("result")]
        public int Result { get; set; }

        /// <summary>
        /// 相似度
        /// </summary>
        [JsonPropertyName("score")]
        public int Score { get; set; }

        /// <summary>
        /// 圖像區域-左上角X
        /// </summary>
        [JsonPropertyName("areaInfo_left")]
        public int AreaInfoLeft { get; set; }

        /// <summary>
        /// 圖像區域-左上角Y
        /// </summary>
        [JsonPropertyName("areaInfo_top")]
        public int AreaInfoTop { get; set; }

        /// <summary>
        /// 圖像區域-寬度
        /// </summary>
        [JsonPropertyName("areaInfo_width")]
        public int AreaInfoWidth { get; set; }

        /// <summary>
        /// 圖像區域-高度
        /// </summary>
        [JsonPropertyName("areaInfo_height")]
        public int AreaInfoHeight { get; set; }

        /// <summary>
        /// 角度
        /// </summary>
        [JsonPropertyName("angle")]
        public float Angle { get; set; }

        /// <summary>
        /// 比對到印章的圖像頁數
        /// </summary>
        [JsonPropertyName("page")]
        public int Page { get; set; }

        /// <summary>
        /// 重合圖像Base64
        /// </summary>
        [JsonPropertyName("resBase64")]
        public string ResBase64 { get; set; }
    }
}
