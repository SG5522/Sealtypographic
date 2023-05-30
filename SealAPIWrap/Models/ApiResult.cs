using System.Text.Json.Serialization;

namespace SealAPIWrap.Models
{
    /// <summary>
    /// 呼叫C++ Dll執行結果
    /// </summary>
    public abstract class ApiResult
    {
        /// <summary>
        /// 執行結果代碼
        /// 0: 執行成功
        /// 1: 執行失敗，見錯誤定義
        /// </summary>
        [JsonPropertyName("retcode")]

        public int RetCode { get; set; }

        /// <summary>
        /// 錯誤說明
        /// </summary>
        [JsonIgnore]
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
