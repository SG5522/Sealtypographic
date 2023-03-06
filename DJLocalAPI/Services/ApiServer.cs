using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using DJLocalAPI.Api;

namespace DJLocalAPI.Api
{
    /// <summary>
    /// Api Server Service 相關設定
    /// </summary>
    public class ApiServer
    {
        private readonly IHostBuilder hostBuilder;
        private IHost? apiServerHost;
        private string apiServerStatus = "Shutdown";
        /// <summary>
        ///  設定網頁及Port號
        /// </summary>
        public ApiServer() : this(null)
        {
        }
        /// <summary>
        /// 設定網頁及Port號
        /// </summary>
        /// <param name="args"></param>
        public ApiServer(string[]? args)
        {
            hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<ApiStartup>(); 
                    webBuilder.UseUrls("http://localhost:22431", "http://localhost:22435");
                });
        }
        /// <summary>
        /// 開啟Server
        /// </summary>
        public async void StartServer(IntPtr handle)
        {
            try
            {
                apiServerHost = hostBuilder.Build();
                await apiServerHost.RunAsync();
                apiServerStatus = "Running";
            }
            catch
            {
                apiServerStatus = "Shutdown";
            }
        }
        /// <summary>
        /// 關閉server
        /// </summary>
        public async void StopServer()
        {
            if(apiServerHost != null)
            {
                await apiServerHost.StopAsync();
                apiServerStatus = "Shutdown";
            }
        }
    }
}
