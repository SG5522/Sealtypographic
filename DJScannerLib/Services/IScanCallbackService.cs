namespace DJScannerLib.Services
{
    /// <summary>
    /// Scan Callback 服務
    /// </summary>
    public interface IScanCallbackService
    {
        /// <summary>
        /// 傳送Base64圖檔資料
        /// </summary>
        /// <param name="base64String">The base64 string.</param>
        /// <param name="alert">if set to <c>true</c> [alert].</param>
        /// <returns></returns>
        Task SendAsync(string base64String, bool alert);

        /// <summary>
        /// 傳送多筆Base64圖檔資料
        /// </summary>
        /// <param name="base64Strings">The base64 strings.</param>
        /// <param name="alert">if set to <c>true</c> [alert].</param>
        /// <returns></returns>
        Task SendAsync(IList<string> base64Strings, bool alert);
    }
}
