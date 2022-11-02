using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SealTypographicWebAPI.Models;
using System.Reflection;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigurationManager config = builder.Configuration; // 取得 IConfiguration

builder.Services.Configure<ScanConfigPath>(
    builder.Configuration.GetSection("ScanConfigPath"));

//addCors
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins(config.GetSection("AllowOrigins").Get<string[]>())
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                          //.AllowAnyOrigin();
                      });
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
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
    //Set the comments path for the Swagger JSON and UI.
    string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
    //@解決部份宣告不為nullable 但還是nullable:true 的問題
    c.SupportNonNullableReferenceTypes();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();

app.MapControllers();

app.Run();
