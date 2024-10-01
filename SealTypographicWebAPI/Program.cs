using CommonLib.Utils;
using DBEntities;
using DBEntities.Utils;
using DJKeycloakAPI.Configs;
using DJKeycloakAPI.Models.Users;
using DJKeycloakLib.Configs;
using DJKeycloakLib.Services;
using Keycloak.AuthServices.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SealTypographicWebAPI.Config;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Services;
using SealTypographicWebAPI.Services.Implements;
using Serilog;
using System.Reflection;

internal class Program
{
    private static void Main(string[] args)
    {
        string allowSpecificOrigins = "allowSpecificOrigins";
        string allowAllOrigins = "allowAllOrigins";

        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(builder.Configuration)
                        .CreateLogger();

        builder.Services.Configure<SystemConfigOption>(
            builder.Configuration.GetSection("SystemConfig"));

        builder.Services.Configure<UploadPathOption>(
            builder.Configuration.GetSection("UploadPath"));

        builder.Services.Configure<SealPathOption>(
            builder.Configuration.GetSection("SealPath"));

        builder.Services.Configure<TemplateImagePathOption>(
            builder.Configuration.GetSection("TemplateImagePath"));

        builder.Services.Configure<TypographyEditImagePathOptions>(
            builder.Configuration.GetSection("TypographyEditImagePath"));

        builder.Services.Configure<KeycloakOptions>(
            builder.Configuration.GetSection("KeycloakAdmin"));

        builder.Services.Configure<LogDatabaseOptions>(
            builder.Configuration.GetSection("LogDatabase"));        

        KeycloakAuthenticationOptions keycloakAuthenticationOptions = new();

        builder.Configuration
            .GetSection(KeycloakAuthenticationOptions.Section)
            .Bind(keycloakAuthenticationOptions, opt => opt.BindNonPublicProperties = true);

        builder.Services.Configure<KeycloakAuthenticationOptions>(builder.Configuration.GetSection(KeycloakAuthenticationOptions.Section));

        //addCors
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: allowAllOrigins,
                              policy =>
                              {
                                  policy.AllowAnyHeader()
                                  .AllowAnyMethod()
                                  .AllowAnyOrigin();
                              });
            string[]? strings = builder.Configuration.GetSection("AllowOrigins").Get<string[]>();
            if (strings != null)
            {
                options.AddPolicy(name: allowSpecificOrigins,
                                    policy =>
                                    {
                                        policy.WithOrigins(strings)
                                                .AllowAnyHeader()
                                                .AllowAnyMethod();
                                    });
            }
        });

        builder.Host.UseSerilog();// <-SeriLog 

        #region -- ConectionString --

        builder.Services.AddDbContextPool<SealTypographicDbContext>(optionsBuilder =>
        {
            string? provider = builder.Configuration.GetValue<string>("Provider");
            switch (provider)
            {
                case "Sqlite":
                    optionsBuilder.UseSqlite(builder.Configuration.GetConnectionString(provider), x => x.MigrationsAssembly(provider));
                    break;
                case "MySql":
                    MySqlServerVersion serverVersion = new(new Version(8, 0, 32));
                    optionsBuilder.UseMySql(builder.Configuration.GetConnectionString(provider), serverVersion, x => x.MigrationsAssembly(provider));
                    break;
                case "MsSql":
                    optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString(provider));
                    break;
                case "PostgreSQL":
                    optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString(provider), x => x.MigrationsAssembly(provider));
                    break;
                default:
                    throw new Exception($"Unsupported provider: {provider}");
            }

#if DEBUG
            optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder =>
            {
                builder.AddConsole().AddDebug();
            }));
#endif
        }, 128);
        #endregion

        #region -- Service --

        builder.Services.AddScoped<ImageService>();
        builder.Services.AddScoped<SealMappingConfigService>();
        builder.Services.AddSingleton<TemplateConfigService>();
        builder.Services.AddScoped<ResponseCodeService>();
        builder.Services.AddScoped<ReviewStatusService>();
        builder.Services.AddScoped<IAdminService, KeycloakAdminService>();
        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
        builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(UserMapperProfile)));

        //DB Process
        builder.Services.AddScoped<ICustomerService, CustomerService>();
        builder.Services.AddScoped<ICustomerSealService, CustomerSealService>();
        builder.Services.AddScoped<ICustomerSealReviewService, CustomerSealReviewService>();
        builder.Services.AddScoped<IAccountantService, AccountantService>();
        builder.Services.AddScoped<IAccountantGroupService, AccountantGroupService>();
        builder.Services.AddScoped<IAccountantGroupMemberService, AccountantGroupMemberService>();
        builder.Services.AddScoped<IAccountantSignService, AcoountantSignService>();
        builder.Services.AddScoped<IAccountantSignReviewService, AccountantSignReviewService>();
        builder.Services.AddScoped<ILetterheadService, LetterheadService>();
        builder.Services.AddScoped<ILetterheadImageService, LetterheadImageService>();
        builder.Services.AddScoped<ITemporarySealService, TemporarySealService>();
        builder.Services.AddScoped<ICustomerSealTemplateService, CustomerSealTemplateService>();
        builder.Services.AddScoped<IAccountantSignTemplateService, AccountantSignTemplateService>();
        builder.Services.AddScoped<ILetterheadImageTemplateService, LetterheadImageTemplateService>();
        builder.Services.AddScoped<IUploadService, UploadService>();
        builder.Services.AddScoped<ITypographicPDFService, TypographicPDFService>();
        builder.Services.AddScoped<IQuarterYearService, QuarterYearService>();
        builder.Services.AddScoped<IImageRangeSettingService, ImageRangeSettingService>();
        builder.Services.AddScoped<IApplicationUserService, ApplicationUserService>();
        builder.Services.AddScoped<ILogReportService, LogReportService>();

        #endregion

        builder.Services.AddLocalization(option => option.ResourcesPath = "Resource");

        string[] supportedCultures = new[] { "en-US", "zh-TW" };
        RequestLocalizationOptions localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
                                                        .AddSupportedCultures(supportedCultures)
                                                        .AddSupportedUICultures(supportedCultures);

        builder.Services.AddMvc()
            .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
            .AddDataAnnotationsLocalization();

        // Add services to the container.
        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString(),
                Title = "SealTypographicWebAPI",
                Description = "取章排版ServerAPI",
            });

            //@解決部份宣告不為nullable 但還是nullable:true 的問題
            c.SupportNonNullableReferenceTypes();

            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"), true);
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{typeof(NewUserForm).Assembly.GetName().Name}.xml"), true);
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{typeof(DJKeycloakLib.Models.BaseModel.PaginateViewModel).Assembly.GetName().Name}.xml"), true);

            OpenApiSecurityScheme securityScheme = new()
            {
                Name = "Auth",
                Type = SecuritySchemeType.OAuth2,
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                },
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"{keycloakAuthenticationOptions.KeycloakUrlRealm}/protocol/openid-connect/auth"),
                        TokenUrl = new Uri($"{keycloakAuthenticationOptions.KeycloakUrlRealm}/protocol/openid-connect/token"),
                        Scopes = new Dictionary<string, string>(),
                    }
                }
            };
            c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
        {securityScheme, Array.Empty<string>()}
            });
        });

        #region -- Authentication --

        builder.Services.AddKeycloakAuthentication(keycloakAuthenticationOptions, options =>
        {
            options.RequireHttpsMetadata = true;
            options.Audience = "account";
            options.BackchannelHttpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = delegate { return true; }
            };
            //TODO:登入日誌每次呼叫API都會被紀錄需要調整
            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = async context =>
                {
                    if (context.Principal!.Identity != null && context.Principal.Identity.IsAuthenticated)
                    {
                        // 登入成功時的處理程序 
                        // 可以在這裡呼叫 LogReportService 來紀錄登入日誌 
                        ILogReportService logReportService = context.HttpContext.RequestServices.GetRequiredService<ILogReportService>();
                        IApplicationUserService applicationUserService = context.HttpContext.RequestServices.GetRequiredService<IApplicationUserService>();

                        UserInfo userInfo = await applicationUserService.GetUserInfo(context.Principal!);

                        // 使用 LogReportService 來紀錄登入日誌 
                        await logReportService.LogLogin(userInfo);
                    }
                }
            };
        });

        //builder.Services.AddKeycloakWebApiAuthentication(builder.Configuration, options =>
        //{
        //    options.RequireHttpsMetadata = true;
        //    options.Audience = "account";
        //    options.BackchannelHttpHandler = new HttpClientHandler
        //    {
        //        ServerCertificateCustomValidationCallback = delegate { return true; }
        //    };
        //});

        //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //            .AddJwtBearer(o =>
        //            {
        //                o.RequireHttpsMetadata = true;
        //                o.MetadataAddress = $"{keycloakAuthenticationOptions.KeycloakUrlRealm}/.well-known/openid-configuration";
        //                o.Authority = $"{keycloakAuthenticationOptions.KeycloakUrlRealm}";
        //                o.Audience = "account";
        //                o.BackchannelHttpHandler = new HttpClientHandler
        //                {
        //                    ServerCertificateCustomValidationCallback = delegate { return true; }
        //                };
        //            });
        #endregion

        builder.Host.UseWindowsService();

        var app = builder.Build();

        if (!app.Environment.IsProduction())
        {
            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swagger, httpReq) =>
                {
                    if (httpReq.Headers.ContainsKey("X-Forwarded-Proto"))
                    {
                        swagger.Servers = new List<OpenApiServer> {
                    new OpenApiServer {
                            Url = $"{httpReq.Headers["X-Forwarded-Proto"]}://{httpReq.Headers["X-Forwarded-Host"]}:{httpReq.Headers["X-Forwarded-Port"]}/{httpReq.Headers["X-Forwarded-Prefix"]}"
                        }};
                    }
                    //else
                    //{
                    //    swagger.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}/{httpReq.Headers["X-Forwarded-Prefix"]}" } };
                    //}            
                });
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "My API V1");
            });

            app.UseCors(allowAllOrigins);
        }
        else
        {
            app.UseCors(allowSpecificOrigins);
        }


        using (IServiceScope scope = app.Services.CreateScope())
        {
            try
            {                
                string privateKeyForder = Path.Combine(Environment.CurrentDirectory, "Keys");
                
                SealTypographicDbContext dbContext = scope.ServiceProvider.GetRequiredService<SealTypographicDbContext>();
                dbContext.Database.Migrate();

                FileUtil.CheckDirectory(privateKeyForder);
                InitialDbData.Initialize(dbContext, privateKeyForder);                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        //app.MapHealthChecks("/healthz");
        //app.UseHealthChecksUI();

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        //app.UseSerilogRequestLogging(); // <-SeriLog 

        app.MapControllers();

        app.Run();
    }
}