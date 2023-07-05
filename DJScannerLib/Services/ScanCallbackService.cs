using Lib.AspNetCore.ServerSentEvents;

namespace DJScannerLib.Services
{
    /// <inheritdoc/>
    public class ScanCallbackService : IScanCallbackService
    {
        private readonly IScanCallbackSSEService scanCallbackSSEService;

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="scanCallbackSSEService"></param>
        public ScanCallbackService(IScanCallbackSSEService scanCallbackSSEService)
        {
            this.scanCallbackSSEService = scanCallbackSSEService;
        }

        /// <inheritdoc/>
        public Task SendAsync(string base64String, bool alert)
        {
            return scanCallbackSSEService.SendEventAsync(new ServerSentEvent
            {
                Type = alert ? "alert" : null,
                Data = new List<string> { base64String }
            });
        }

        /// <inheritdoc/>
        public Task SendAsync(IList<string> base64Strings, bool alert)
        {
            return scanCallbackSSEService.SendEventAsync(new ServerSentEvent
            {
                Type = alert ? "alert" : null,
                Data = base64Strings
            });
        }
    }
}
