using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SealAPIWrap.Models
{
    /// <summary>
    /// 驗印結果輔助顯示結果資料
    /// </summary>
    public class SealShowResult : ApiResult
    {
        /// <summary>
        /// 輪廓圖Base64
        /// </summary>
        [JsonPropertyName("lunkbase64")]
        public string OutlineBase64 { get; set; }

        /// <summary>
        /// 折角圖Base64
        /// </summary>
        [JsonPropertyName("zhejbase64")]
        public string ChamferBase64 { get; set; }

        /// <summary>
        /// 殘像對比: 原始印鑑Base64
        /// </summary>
        [JsonPropertyName("libDatabase64")]
        public string AfterimageSourceBase64 { get; set; }

        /// <summary>
        /// 殘像對比: 提取印鑑Base64
        /// </summary>
        [JsonPropertyName("tarDatabase64")]
        public string AfterimageBase64 { get; set; }

        /// <summary>
        /// 殘像對比: 差異圖Base64
        /// </summary>
        [JsonPropertyName("diffDatabase64")]
        public string AfterimageDiffBase64 { get; set; }

    }
}
