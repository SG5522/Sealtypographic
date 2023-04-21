using DJLocalAPI;
using Microsoft.Extensions.Configuration;
using Serilog;

//@net core net5 net6 對unicode 以外文字的支援不足必須加這段
System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

ApplicationConfiguration.Initialize();
Application.Run(new FrmDJLLocalAPI(args));