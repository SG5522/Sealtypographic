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

        /// <summary>
        /// Sends the asynchronous.
        /// </summary>
        /// <param name="base64Strings">The base64 strings.</param>
        /// <param name="alert">if set to <c>true</c> [alert].</param>
        /// <returns></returns>
        Task SendAsync(IList<string> base64Strings, bool alert);
    }
}
