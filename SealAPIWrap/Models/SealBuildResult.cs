using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SealAPIWrap.Models
{
    /// <summary>
    /// 建印結果資料
    /// </summary>
    public class SealBuildResult : ApiResult
    {
        /// <summary>
        /// 印章数量
        /// </summary>
        [JsonPropertyName("sealno")]
        public int SealNo { get; set; }

        /// <summary>
        /// 建印結果細項資料
        /// </summary>
        [JsonPropertyName("seallibs")]
        public IList<SealBuildDetail> Seals { get; set; }
    }
}
