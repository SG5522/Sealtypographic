using Microsoft.OpenApi.Models;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Services;
using SealTypographicWebAPI.Entities;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SealTypographicWebAPI.Services.Implements;
using SealTypographicWebAPI.Config;

string allowSpecificOrigins = "allowSpecificOrigins";
string allowAllOrigins = "allowSpecificOrigins";

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigurationManager config = builder.Configuration; // 取得 IConfiguration
Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();

builder.Services.Configure<ScanConfigPath>(
    builder.Configuration.GetSection("ScanConfigPath"));

builder.Services.Configure<SealConfigPath>(
    builder.Configuration.GetSection("SealPath"));

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
                          policy.WithOrigins(config.GetSection("AllowOrigins").Get<string[]>())
                          .AllowAnyHeader()
                          .AllowAnyMethod();                          
                      });
});


builder.Host.UseSerilog();// <-SeriLog 

#region -- ConectionString --
builder.Services.AddDbContextPool<SealTypographicDbContext>(optionsBuilder =>
{    
    optionsBuilder.UseSqlite(config.GetConnectionString("Sqlite"));
    //MySqlServerVersion serverVersion = new(new Version(5, 7, 27));
    //optionsBuilder.UseMySql(config.GetConnectionString("MySql"), serverVersion);
},128);
#endregion

#region -- Service --

builder.Services.AddSingleton<ImageSharpService>();
builder.Services.AddSingleton<UploadService>();
builder.Services.AddAutoMapper(typeof(MapperProfile));

//DB Process
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICustomerSealService, CustomerSealService>();
builder.Services.AddScoped<ICustomerSealReviewService, CustomerSealReviewService>();
builder.Services.AddScoped<IAccountantService, AccountantService>();
builder.Services.AddScoped<IAccountantGroupService, AccountantGroupService>();
builder.Services.AddScoped<IAccountantGroupMemberService, AccountantGroupMemberService>();
builder.Services.AddScoped<IAccountantSignService, AcoountantSignService>();
builder.Services.AddScoped<ILetterheadService, LetterheadService>();
builder.Services.AddScoped<ILetterheadImageService, LetterheadImageService>();
builder.Services.AddScoped<SealMappingConfigService>();

#endregion



// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
    //Set the comments path for the Swagger JSON and UI.
    string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
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

});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
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
    SealTypographicDbContext dbContext = scope.ServiceProvider.GetRequiredService<SealTypographicDbContext>();
    dbContext.Database.Migrate();
}

app.UseAuthorization();
//app.UseSerilogRequestLogging(); // <-SeriLog 

app.MapControllers();

app.Run();
