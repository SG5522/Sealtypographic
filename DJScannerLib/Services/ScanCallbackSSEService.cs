using Lib.AspNetCore.ServerSentEvents;
using Microsoft.Extensions.Options;

namespace DJScannerLib.Services
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Lib.AspNetCore.ServerSentEvents.ServerSentEventsService" />
    /// <seealso cref="DJScannerLib.Services.IScanCallbackSSEService" />
    public class ScanCallbackSSEService : ServerSentEventsService, IScanCallbackSSEService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScanCallbackSSEService"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public ScanCallbackSSEService(IOptions<ServerSentEventsServiceOptions<ScanCallbackSSEService>> options) 
            : base(options.ToBaseServerSentEventsServiceOptions())
        {
        }
    }
}
