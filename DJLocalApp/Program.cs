using DJScannerLib.Services;
using Lib.AspNetCore.ServerSentEvents;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

namespace DJLocalApp
{
    internal static class Program
    {
        public static WebApplication WebApp;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            string allowAllOrigins = "allowAllOrigins";

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            builder.Host.UseSerilog();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: allowAllOrigins,
                                  policy =>
                                  {
                                      policy.AllowAnyHeader()
                                      .AllowAnyMethod()
                                      .AllowAnyOrigin();
                                  });
            });

            builder.Services.AddServerSentEvents();
            builder.Services.AddResponseCompression(options =>
            {
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "text/event-stream" });
            });

            builder.Services.AddControllers();

            builder.Services.AddScoped<FrmMain>();
            builder.Services.AddSingleton<IHostedService, Services.HealthCheckService>();
            builder.Services.AddServerSentEvents<IScanCallbackSSEService, ScanCallbackSSEService>(options =>
            {
                options.ReconnectInterval = 5000;
            });
            builder.Services.AddTransient<IScanCallbackService, ScanCallbackService>();
            builder.Services.AddSingleton<IScannerService, ScannerService>();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString(),
                    Title = "D & J Local API",
                    Description = "D & J Image Corp. Local API",
                });

                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"), true);
            });

            WebApp = builder.Build();
            WebApp.UseSwagger();
            WebApp.UseSwaggerUI();
            WebApp.UseCors(allowAllOrigins);
            WebApp.UseAuthorization();

            WebApp.MapControllers();
            WebApp.UseResponseCompression().UseRouting().UseEndpoints(endpoints =>
            {
                endpoints.MapServerSentEvents("/health-check");

                endpoints.MapServerSentEvents<ScanCallbackSSEService>("/scan-callback");
            });

            WebApp.Urls.Add("http://localhost:22431");
            WebApp.Urls.Add("http://localhost:22432");
            WebApp.Urls.Add("http://localhost:22433");
            WebApp.Urls.Add("http://localhost:22434");
            WebApp.Urls.Add("http://localhost:22435");

            WebApp.RunAsync();

            using IServiceScope serviceScope = WebApp.Services.CreateScope();
            IServiceProvider serviceProvider = serviceScope.ServiceProvider;
            try
            {
                FrmMain frmMain = serviceProvider.GetRequiredService<FrmMain>();
                Application.Run(frmMain);

                Console.WriteLine("Success");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Occured");
            }
            //ApplicationConfiguration.Initialize();
            //Application.Run(new FrmMain());
        }
    }
}