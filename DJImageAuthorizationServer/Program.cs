using DJImageAuthorizationServer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DJImageAuthorizationServer.Fido2;
using Fido2NetLib;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using static OpenIddict.Abstractions.OpenIddictConstants;
using System.Security.Claims;
using Quartz;
using DJImageAuthorizationServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    // Configure the context to use Microsoft SQL Server.    
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));

    // Register the entity sets needed by OpenIddict.
    // Note: use the generic overload if you need
    // to replace the default OpenIddict entities.
    options.UseOpenIddict();
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
  .AddEntityFrameworkStores<ApplicationDbContext>()
  .AddDefaultTokenProviders()
  .AddDefaultUI()
  .AddTokenProvider<Fido2UserTwoFactorTokenProvider>("FIDO2");

builder.Services.Configure<Fido2Configuration>(builder.Configuration.GetSection("fido2"));
builder.Services.AddScoped<Fido2Store>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.Configure<IdentityOptions>(options =>
{
    // Configure Identity to use the same JWT claims as OpenIddict instead
    // of the legacy WS-Federation claims it uses by default (ClaimTypes),
    // which saves you from doing the mapping in your authorization controller.
    options.ClaimsIdentity.UserNameClaimType = Claims.Name;
    options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
    options.ClaimsIdentity.RoleClaimType = Claims.Role;
    options.ClaimsIdentity.EmailClaimType = Claims.Email;

    // Note: to require account confirmation before login,
    // register an email sender service (IEmailSender) and
    // set options.SignIn.RequireConfirmedAccount to true.
    //
    // For more information, visit https://aka.ms/aspaccountconf.
    options.SignIn.RequireConfirmedAccount = false;
});

// OpenIddict offers native integration with Quartz.NET to perform scheduled tasks
// (like pruning orphaned authorizations/tokens from the database) at regular intervals.
builder.Services.AddQuartz(options =>
{
    options.UseMicrosoftDependencyInjectionJobFactory();
    options.UseSimpleTypeLoader();
    options.UseInMemoryStore();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder
                .AllowCredentials()
                .WithOrigins(
                             "https://localhost:4200", "https://localhost:4204", "*")
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// Register the Quartz.NET service and configure it to block shutdown until jobs are complete.
builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddOpenIdConnect("KeyCloak", "KeyCloak", options =>
    {
        options.SignInScheme = "Identity.External";
        //Keycloak server
        options.Authority = builder.Configuration.GetSection("Keycloak")["ServerRealm"];
        //Keycloak client ID
        options.ClientId = builder.Configuration.GetSection("Keycloak")["ClientId"];
        //Keycloak client secret in user secrets for dev
        options.ClientSecret = builder.Configuration.GetSection("Keycloak")["ClientSecret"];
        //Keycloak .wellknown config origin to fetch config
        options.MetadataAddress = builder.Configuration.GetSection("Keycloak")["Metadata"];
        //Require keycloak to use SSL

        options.GetClaimsFromUserInfoEndpoint = true;
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.SaveTokens = true;
        options.ResponseType = OpenIdConnectResponseType.Code;
        options.RequireHttpsMetadata = false; //dev

        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = "name",
            RoleClaimType = ClaimTypes.Role,
            ValidateIssuer = true
        };
    });

builder.Services.AddOpenIddict()
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore()
               .UseDbContext<ApplicationDbContext>();

        options.UseQuartz();
    })
    .AddServer(options =>
    {
        // Enable the authorization, logout, token and userinfo endpoints.
        options.SetAuthorizationEndpointUris("connect/authorize")           
           .SetIntrospectionEndpointUris("connect/introspect")
           .SetLogoutEndpointUris("connect/logout")
           .SetTokenEndpointUris("connect/token")
           .SetUserinfoEndpointUris("connect/userinfo")
           .SetVerificationEndpointUris("connect/verify");

        options.AllowAuthorizationCodeFlow()
               .AllowHybridFlow()
               .AllowClientCredentialsFlow()
               .AllowRefreshTokenFlow();

        options.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Roles, "dataEventRecords");

        options.AddDevelopmentEncryptionCertificate()
               .AddDevelopmentSigningCertificate();

        options.UseAspNetCore()
               .EnableAuthorizationEndpointPassthrough()
               .EnableLogoutEndpointPassthrough()
               .EnableTokenEndpointPassthrough()
               .EnableUserinfoEndpointPassthrough()
               .EnableStatusCodePagesIntegration();
    })
    .AddValidation(options =>
    {
        options.UseLocalServer();
        options.UseAspNetCore();
    });

builder.Services.AddHostedService<Worker>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseStatusCodePagesWithReExecute("~/error");
    //app.UseExceptionHandler("~/error");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapDefaultControllerRoute();
    endpoints.MapRazorPages();
});

app.Run();
