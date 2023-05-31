using DJLocalApp.Extensions;
using DJScannerLib.Models;
using DJScannerLib.Services;
using Lib.AspNetCore.ServerSentEvents;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SealAPIWrap;
using SealAPIWrap.Models;
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

            // 增加SSE(Server-Sent Events)所需的Content Type
            builder.Services.AddResponseCompression(options =>
            {
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "text/event-stream" });
            });

            builder.Services.AddControllers();

            builder.Services.AddScoped<FrmMain>();

            // 預設的SSE(Server-Sent Events)
            builder.Services.AddServerSentEvents();
            // Scan Callback SSE
            builder.Services.AddServerSentEvents<IScanCallbackSSEService, ScanCallbackSSEService>();

            // 健康檢查服務(提供預設SSE使用)
            builder.Services.AddSingleton<IHostedService, Services.HealthCheckService>();
            builder.Services.AddSingleton<IScanCallbackService, ScanCallbackService>();
            builder.Services.AddSingleton<IScannerService, ScannerService>();
            builder.Services.AddSingleton<ISealService, SealService>();

            builder.Services.AddAutoMapper(typeof(MapperProfile));

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString(),
                    Title = "D & J Local API",
                    Description = "D & J Image Corp. Local API",
                });

                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"), true);
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{typeof(BaseResult).Assembly.GetName().Name}.xml"), true);
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{typeof(ApiRequest).Assembly.GetName().Name}.xml"), true);
            });

            WebApp = builder.Build();
            WebApp.UseSwagger();
            WebApp.UseSwaggerUI();
            WebApp.UseCors(allowAllOrigins);
            WebApp.UseAuthorization();

            WebApp.MapControllers();
            // SSE所需要的url path
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