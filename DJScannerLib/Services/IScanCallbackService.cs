namespace DJScannerLib.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IScanCallbackService
    {
        /// <summary>
        /// Sends the asynchronous.
        /// </summary>
        /// <param name="base64String">The base64 string.</param>
        /// <param name="alert">if set to <c>true</c> [alert].</param>
        /// <returns></returns>
        Task SendAsync(string base64String, bool alert);
    }
}
