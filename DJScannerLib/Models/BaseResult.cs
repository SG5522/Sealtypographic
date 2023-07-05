namespace DJScannerLib.Models
{
    /// <summary>
    /// 共用結果
    /// </summary>
    public class BaseResult
    {
        /// <summary>
        /// 結果: 成功 true，失敗 false
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
