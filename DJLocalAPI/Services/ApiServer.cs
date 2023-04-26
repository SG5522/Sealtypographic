using DJScannerLib.Configs;
using DJScannerLib.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

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
        public ServerStatus Status
        {
            get
            {
                return apiServerHost != null ? ServerStatus.Running : ServerStatus.Shutdown;
            }
        }

        private readonly IHost? apiServerHost;

        /// <summary>
        /// 建構 - 設定網頁及Port
        /// </summary>
        /// <param name="args"></param>
        public ApiServer(string[] args, IntPtr handle)
        {
            //ScannerService = new ScannerService(handle);
            apiServerHost = Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices(services => 
                    {
                        services.Configure<FormOptions>(o => new FormOptions
                        {
                            Handle = handle
                        });
                        //services.AddScoped(sp => ScannerService);
                    })
                    .UseStartup<ApiStartup>()
                    .UseUrls("http://localhost:22431", "http://localhost:22432", "http://localhost:22433", "http://localhost:22434", "http://localhost:22435");
                }).Build();
        }

        /// <summary>
        /// 開啟Server
        /// </summary>
        public void StartAsync()
        {
            apiServerHost.RunAsync();
        }

        /// <summary>
        /// 關閉server
        /// </summary>
        public void StopAsync()
        {
            apiServerHost.StopAsync();
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
