using System.Text.Json.Serialization;

namespace SealAPIWrap.Models
{
    /// <summary>
    /// 圖像處理Request傳入資料
    /// </summary>
    public class ImageProcessRequest : ApiRequest
    {
        /// <summary>
        /// 裁剪區域-左上角X
        /// </summary>
        [JsonPropertyName("areaInfo_left")]
        public int? AreaInfoLeft { get; set; }

        /// <summary>
        /// 裁剪區域-左上角Y
        /// </summary>
        [JsonPropertyName("areaInfo_top")]
        public int? AreaInfoTop { get; set; }

        /// <summary>
        /// 裁剪區域-寬度
        /// </summary>
        [JsonPropertyName("areaInfo_width")]
        public int? AreaInfoWidth { get; set; }

        /// <summary>
        /// 裁剪區域-高度
        /// </summary>
        [JsonPropertyName("areaInfo_height")]
        public int? AreaInfoHeight { get; set; }

        /// <summary>
        /// 圖像原始dpi
        /// </summary>
        [JsonPropertyName("dpi_src")]
        public int? SourceDPI { get; set; }

        /// <summary>
        /// 圖像目標dpi
        /// </summary>
        [JsonPropertyName("dpi_det")]
        public int? TargetDPI { get; set; }

        /// <summary>
        /// 旋轉角度：0-360
        /// </summary>
        [JsonPropertyName("angle")]
        public int? Angle { get; set; }
    }
}
