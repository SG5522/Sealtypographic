using Lib.AspNetCore.ServerSentEvents;

namespace DJScannerLib.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ScanCallbackService : IScanCallbackService
    {
        private readonly IScanCallbackSSEService scanCallbackSSEService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanCallbackService"/> class.
        /// </summary>
        /// <param name="scanCallbackSSEService">The scan callback sse service.</param>
        public ScanCallbackService(IScanCallbackSSEService scanCallbackSSEService)
        {
            this.scanCallbackSSEService = scanCallbackSSEService;
        }

        /// <summary>
        /// Sends the asynchronous.
        /// </summary>
        /// <param name="base64String">The base64 string.</param>
        /// <param name="alert">if set to <c>true</c> [alert].</param>
        /// <returns></returns>
        public Task SendAsync(string base64String, bool alert)
        {
            return scanCallbackSSEService.SendEventAsync(new ServerSentEvent
            {
                Type = alert ? "alert" : null,
                Data = new List<string> { base64String }
            });
        }
    }
}
