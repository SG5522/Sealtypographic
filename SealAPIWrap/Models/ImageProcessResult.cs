using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SealAPIWrap.Models
{
    /// <summary>
    /// 圖像處理結果資料
    /// </summary>
    public class ImageProcessResult : ApiResult
    {
        /// <summary>
        /// 圖像Base64
        /// </summary>
        [JsonPropertyName("imgbase64")]
        public string ImgBase64 { get; set; }
    }
}
