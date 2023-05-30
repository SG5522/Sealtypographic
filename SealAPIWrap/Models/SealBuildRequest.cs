using System.Text.Json.Serialization;

namespace SealAPIWrap.Models
{
    /// <summary>
    /// 建印Request傳入資料
    /// </summary>
    public class SealBuildRequest : ApiRequest
    {
        /// <summary>
        /// 顏色: 0-紅色，1-藍色，2-黑色(指定區域建印專用)
        /// </summary>
        [JsonPropertyName("color")]
        public Color Color { get; set; }

        /// <summary>
        /// 框選區域-左上角X(指定區域建印使用)
        /// 自動分離：輸入0。
        /// </summary>
        [JsonPropertyName("areaInfo_left")]
        public int AreaInfoLeft { get; set; }

        /// <summary>
        /// 框選區域-左上角Y(指定區域建印使用)
        /// 自動分離：輸入0。
        /// </summary>
        [JsonPropertyName("areaInfo_top")]
        public int AreaInfoTop { get; set; }

        /// <summary>
        /// 框選區域-寬度(指定區域建印使用)
        /// 自動分離：輸入0。
        /// </summary>
        [JsonPropertyName("areaInfo_width")]
        public int AreaInfoWidth { get; set; }

        /// <summary>
        /// 框選區域-高度(指定區域建印使用)
        /// 自動分離：輸入0。
        /// </summary>
        [JsonPropertyName("areaInfo_height")]
        public int AreaInfoHeight { get; set; }

        /// <summary>
        /// 圖像dpi
        /// </summary>
        [JsonPropertyName("dpi")]
        public int DPI { get; set; }

        /// <summary>
        /// 是否對分離印鑑進行旋轉：0-不旋轉，1-自動旋轉
        /// </summary>
        [JsonPropertyName("rotate")]
        public Rotate Rotate { get; set; }
    }
}
