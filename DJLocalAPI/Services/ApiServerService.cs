using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using DJLocalAPI.Api;

namespace DJLocalAPI.Api
{
    public class ApiServerService
    {
        private readonly IHostBuilder hostBuilder;
        private IHost? apiServerHost;
        private string apiServerStatus = "Shutdown";
        //private readonly string[] apiUrls = new string[] { "http://localhost:5123", ""};

        public ApiServerService() : this(null)
        {
        }

        public ApiServerService(string[]? args)
        {
            hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<ApiStartup>(); 
                    webBuilder.UseUrls("http://localhost:22431", "http://localhost:22435");
                });
        }

        public async void StartServer()
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
