using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SealAPIWrap.Models
{
    /// <summary>
    /// 驗印Request傳入資料
    /// </summary>
    public class SealIdentifyRequest : ApiRequest
    {
        /// <summary>
        /// 檔案類型
        /// </summary>
        [JsonPropertyName("filetype")]
        public FileType FileType { get; set; }

        /// <summary>
        /// 顏色: 0-紅色，1-藍色，2-黑色
        /// </summary>
        [JsonPropertyName("color")]
        public Color Color { get; set; }

        /// <summary>
        /// 圖像dpi
        /// </summary>
        [JsonPropertyName("dpi")]
        public int DPI { get; set; }

        /// <summary>
        /// 驗印鬆緊度
        /// </summary>
        [JsonPropertyName("tightLevel")]
        public TightLevel TightLevel { get; set; }

        /// <summary>
        /// 框選區域-左上角X(指定區域驗印使用)
        /// 自動驗印：輸入0。
        /// </summary>
        [JsonPropertyName("areaInfo_left")]
        public int AreaInfoLeft { get; set; }

        /// <summary>
        /// 框選區域-左上角Y(指定區域驗印使用)
        /// 自動驗印：輸入0。
        /// </summary>
        [JsonPropertyName("areaInfo_top")]
        public int AreaInfoTop { get; set; }

        /// <summary>
        /// 框選區域-高度(指定區域驗印使用)
        /// 自動驗印：輸入0。
        /// </summary>
        [JsonPropertyName("areaInfo_width")]
        public int AreaInfoWidth { get; set; }

        /// <summary>
        /// 框選區域-寬度(指定區域驗印使用)
        /// 自動驗印：輸入0。
        /// </summary>
        [JsonPropertyName("areaInfo_height")]
        public int AreaInfoHeight { get; set; }

        /// <summary>
        /// 驗印傳入細項資料
        /// </summary>
        [JsonPropertyName("seallibs")]
        public IList<SealIdentifyItem> Seals { get; set; }
    }
}
