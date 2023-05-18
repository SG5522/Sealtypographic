using Microsoft.OpenApi.Models;
using SealTypographicWebAPI.Services;
using DBEntities;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SealTypographicWebAPI.Services.Implements;
using SealTypographicWebAPI.Config;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting.WindowsServices;
using SealTypographicWebAPI.Models.CustomerSealTemplate;
using DBEntitiesExtension;
using Microsoft.Extensions.Options;

string allowSpecificOrigins = "allowSpecificOrigins";
string allowAllOrigins = "allowAllOrigins";

Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

builder.Services.Configure<UploadPathOption>(
    builder.Configuration.GetSection("UploadPath"));

builder.Services.Configure<SealPathOption>(
    builder.Configuration.GetSection("SealPath"));

builder.Services.Configure<TemplateImagePathOption>(
    builder.Configuration.GetSection("TemplateImagePath"));

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
    options.AddPolicy(name: allowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins(builder.Configuration.GetSection("AllowOrigins").Get<string[]>())
                          .AllowAnyHeader()
                          .AllowAnyMethod();                          
                      });
});


builder.Host.UseSerilog();// <-SeriLog 

#region -- ConectionString --
#if DEBUG
builder.Services.AddDbContextPool<SealTypographicExtensionDbContext>(optionsBuilder =>
{
    optionsBuilder.UseSqlite(builder.Configuration.GetConnectionString("Sqlite2"), x => x.MigrationsAssembly("Sqlite"));

    optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder =>
    {
        builder.AddConsole().AddDebug();
    }));
}, 128);
#endif

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
builder.Services.AddAutoMapper(typeof(MapperProfile));

//DB Process
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICustomerSealService, CustomerSealService>();
builder.Services.AddScoped<ICustomerSealReviewService, CustomerSealReviewService>();
builder.Services.AddScoped<IAccountantService, AccountantService>();
builder.Services.AddScoped<IAccountantGroupService, AccountantGroupService>();
builder.Services.AddScoped<IAccountantGroupMemberService, AccountantGroupMemberService>();
builder.Services.AddScoped<IAccountantSignReviewService, AccountantSignReviewService>();
builder.Services.AddScoped<IAccountantSignService, AcoountantSignService>();
builder.Services.AddScoped<ILetterheadService, LetterheadService>();
builder.Services.AddScoped<ILetterheadImageService, LetterheadImageService>();
builder.Services.AddScoped<ITemporarySealService, TemporarySealService>();
builder.Services.AddScoped<ICustomerSealTemplateService, CustomerSealTemplateService>();
builder.Services.AddScoped<IAccountantSignTemplateService, AccountantSignTemplateService>();
builder.Services.AddScoped<ILetterheadImageTemplateService, LetterheadImageTemplateService>();
builder.Services.AddScoped<ITypographicPDFService, TypographicPDFService>();
builder.Services.AddScoped<UploadService>();

#endregion

#region -- Authentication --
//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddOpenIdConnect(options =>
//{
//    options.Authority = identityUrl.ToString();

//})
//;
#endregion

builder.Services.AddLocalization(option => option.ResourcesPath = "Resource");

string[] supportedCultures = new[] { "en-US", "zh-TW"  };
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
    //Set the comments path for the Swagger JSON and UI.
    string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    c.SwaggerDoc("v1", new OpenApiInfo
    {        
        Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString(),
        Title = "SealTypographicWebAPI",
        Description = "取章排版ServerAPI",
        //TermsOfService = new Uri("https://example.com/terms"),
        //Contact = new OpenApiContact
        //{
        //    Name = "Shayne Boyer",
        //    Email = string.Empty,
        //    Url = new Uri("https://twitter.com/spboyer"),
        //},
        //License = new OpenApiLicense
        //{
        //    Name = "Use under LICX",
        //    Url = new Uri("https://example.com/license"),
        //}
    });   

    //@解決部份宣告不為nullable 但還是nullable:true 的問題
    c.SupportNonNullableReferenceTypes();

    c.IncludeXmlComments(xmlPath,true);
    
    //c.SchemaFilter<EnumSchemaFilter>();
});

builder.Host.UseWindowsService();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    //app.UseStaticFiles();
    app.UseSwagger();
    app.UseSwaggerUI();

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
        SealTypographicDbContext dbContext = scope.ServiceProvider.GetRequiredService<SealTypographicDbContext>();
        dbContext.Database.Migrate();
        InitialDbData.Initialize(dbContext);

#if DEBUG
        SealTypographicExtensionDbContext dbContext2 = scope.ServiceProvider.GetRequiredService<SealTypographicExtensionDbContext>();
        dbContext2.Database.Migrate();
        InitialDb2Data.Initialize(dbContext2);
#endif

    }
    catch(Exception ex)
    {        
        Console.WriteLine(ex.ToString());
    }
}


app.UseAuthorization();
//app.UseSerilogRequestLogging(); // <-SeriLog 

app.MapControllers();

app.Run();
