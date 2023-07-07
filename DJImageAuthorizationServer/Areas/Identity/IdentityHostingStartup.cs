using DJImageAuthorizationServer.Entities;

[assembly: HostingStartup(typeof(DJImageAuthorizationServer.Areas.Identity.IdentityHostingStartup))]
namespace DJImageAuthorizationServer.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}
