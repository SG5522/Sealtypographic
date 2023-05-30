using System.Text.Json.Serialization;

namespace SealAPIWrap.Models
{
    /// <summary>
    /// Request傳入資料
    /// </summary>
    public abstract class ApiRequest
    {
        /// <summary>
        /// 圖像Base64
        /// </summary>
        [JsonPropertyName("imgbase64")]
        public string ImgBase64 { get; set; }
    }
}
