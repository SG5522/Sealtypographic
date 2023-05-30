using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SealAPIWrap.Models
{
    /// <summary>
    /// 驗印結果資料
    /// </summary>
    public class SealIdentifyResult : ApiResult
    {
        /// <summary>
        /// 印章数量
        /// </summary>
        [JsonPropertyName("sealno")]
        public int SealNo { get; set; }

        /// <summary>
        /// 驗印結果細項資料
        /// </summary>
        [JsonPropertyName("seallibs")]
        public IList<SealIdentifyDetail> Seals { get; set; }
    }
}
