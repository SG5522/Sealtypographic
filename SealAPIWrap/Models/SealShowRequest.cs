using System.Text.Json.Serialization;

namespace SealAPIWrap.Models
{
    /// <summary>
    /// 驗印結果輔助顯示Request傳入資料
    /// </summary>
    public class SealShowRequest : ApiRequest
    {
        /// <summary>
        /// 操作類型: 1-輪廓圖查看，2-折角圖查看，3-殘像對比查看
        /// </summary>
        [JsonPropertyName("type")]
        public ShowType ShowType { get; set; }

        /// <summary>
        /// 折角圖角度：可輸入45,90,135,180...
        /// </summary>
        [JsonPropertyName("rotate")]
        public int? Angle { get; set; }
    }
}
