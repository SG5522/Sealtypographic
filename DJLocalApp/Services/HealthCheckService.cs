using Lib.AspNetCore.ServerSentEvents;
using Microsoft.Extensions.Hosting;

namespace DJLocalApp.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class HealthCheckService : BackgroundService
    {
        #region Fields
        private const string HEARTBEAT_MESSAGE_FORMAT = "D & J Image Local API V. {0} Heart Check ({1})";

        private readonly IServerSentEventsService serverSentEventsService;
        #endregion

        #region Constructor        
        /// <summary>
        /// Initializes a new instance of the <see cref="HealthCheckService"/> class.
        /// </summary>
        /// <param name="serverSentEventsService">The server sent events service.</param>
        public HealthCheckService(IServerSentEventsService serverSentEventsService)
        {
            this.serverSentEventsService = serverSentEventsService;
        }
        #endregion

        #region Methods

        /// <summary>
        /// This method is called when the <see cref="T:Microsoft.Extensions.Hosting.IHostedService" /> starts. The implementation should return a task that represents
        /// the lifetime of the long running operation(s) being performed.
        /// </summary>
        /// <param name="stoppingToken">Triggered when <see cref="M:Microsoft.Extensions.Hosting.IHostedService.StopAsync(System.Threading.CancellationToken)" /> is called.</param>
        /// <remarks>
        /// See <see href="https://docs.microsoft.com/dotnet/core/extensions/workers">Worker Services in .NET</see> for implementation guidelines.
        /// </remarks>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await serverSentEventsService.SendEventAsync(String.Format(HEARTBEAT_MESSAGE_FORMAT, this.GetType().Assembly.GetName().Version, DateTime.Now.ToLocalTime()));

                await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
            }
        }
        #endregion
    }
}
