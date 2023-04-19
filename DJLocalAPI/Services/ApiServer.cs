using DJScannerLib.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ScannerLib.Services;

namespace DJLocalAPI.Api
{
    /// <summary>
    /// Api Server Service 相關設定
    /// </summary>
    public class ApiServer
    {
        /// <summary>
        /// 
        /// </summary>
        public IScannerService ScannerService;
        
        /// <summary>
        /// 
        /// </summary>
        public ServerStatus apiServerStatus = ServerStatus.Shutdown;

        private readonly IHost? apiServerHost;

        /// <summary>
        /// 建構 - 設定網頁及Port
        /// </summary>
        /// <param name="args"></param>
        public ApiServer(string[] args, IntPtr handle)
        {
            ScannerService = new ScannerService(handle);
            apiServerHost = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices(services => 
                    {
                        services.AddScoped(sp => ScannerService);
                    })
                    .UseStartup<ApiStartup>()
                    .UseUrls("http://localhost:22431", "http://localhost:22435");
                }).Build();
        }

        /// <summary>
        /// 開啟Server
        /// </summary>
        public void StartServer()
        {
            _ = apiServerHost.RunAsync().ContinueWith(antecedent =>
            {
                apiServerStatus = apiServerHost != null ? ServerStatus.Running : ServerStatus.Shutdown;
            });
        }

        /// <summary>
        /// 關閉server
        /// </summary>
        public void StopServer()
        {
            if(apiServerHost != null)
            {
                _ = apiServerHost.StopAsync().ContinueWith(antecedent => apiServerStatus = ServerStatus.Shutdown);
            }
        }
    }

    /// <summary>
    /// API Server狀態
    /// </summary>
    public enum ServerStatus
    {
        /// <summary>
        /// 執行中
        /// </summary>
        Running,
        /// <summary>
        /// 關機
        /// </summary>
        Shutdown
    }
}
