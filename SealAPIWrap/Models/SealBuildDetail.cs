using System.Text.Json.Serialization;

namespace SealAPIWrap.Models
{
    /// <summary>
    /// 建印細項資料
    /// </summary>
    public class SealBuildDetail
    {
        /// <summary>
        /// 編號
        /// </summary>
        [JsonPropertyName("index")]
        public int Index { get; set; }

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
        /// 顏色
        /// </summary>
        [JsonPropertyName("color")]
        public int Color { get; set; }

        [JsonIgnore]
        public Color ColorEnum 
        { 
            get
            {
                return (Color)Color;
            }
        }

        /// <summary>
        /// 圖像Base64
        /// </summary>
        [JsonPropertyName("stampBase64")]
        public string StampBase64 { get; set; }
    }
}
