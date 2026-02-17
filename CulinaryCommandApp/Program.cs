using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using CulinaryCommand.Data;
using CulinaryCommand.Services;
using CulinaryCommand.Inventory.Services;
using CulinaryCommand.PurchaseOrder.Services;
using CulinaryCommand.Components;
using CulinaryCommand.Inventory;
using CulinaryCommand.Inventory.Services.Interfaces;
using CulinaryCommandApp.AIDashboard.Services.Reporting;
using Google.GenAI;
using System;
using CulinaryCommand.Services.UserContextSpace;
using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.NETCore.Setup;
using CulinaryCommandApp.Services;



var builder = WebApplication.CreateBuilder(args);

//
// =====================
// UI
// =====================
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//
// =====================
// Cognito Authentication (MUST be before Build)
// =====================
// ===== Cognito Auth (OIDC) =====
builder.Services
  .AddAuthentication(options =>
  {
      options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
  })
  .AddCookie()
  .AddOpenIdConnect(options =>
  {
    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
      var userPoolId = "us-east-2_SULe0c9vr";
      var region = "us-east-2";

      options.Authority = $"https://cognito-idp.{region}.amazonaws.com/{userPoolId}";
      options.MetadataAddress = $"{options.Authority}/.well-known/openid-configuration";

      options.ClientId = "55joip0viah9qtj7dndhvma2gt";
      var cognitoClientId = builder.Configuration["Authentication:Cognito:ClientId"];
        var cognitoSecretFromEnv = Environment.GetEnvironmentVariable("COGNITO_CLIENT_SECRET");
        var cognitoSecretFromConfig = builder.Configuration["Authentication:Cognito:ClientSecret"];

        var cognitoClientSecret =
            !string.IsNullOrWhiteSpace(cognitoSecretFromEnv) ? cognitoSecretFromEnv :
            cognitoSecretFromConfig;

        options.ClientId = cognitoClientId;
        options.ClientSecret = cognitoClientSecret;


      options.ResponseType = OpenIdConnectResponseType.Code;
      options.SaveTokens = true;

      options.CallbackPath = "/signin-oidc";
      options.SignedOutCallbackPath = "/signout-callback-oidc";

      options.RequireHttpsMetadata = true; // keep true

      options.Scope.Clear();
      options.Scope.Add("openid");
      options.Scope.Add("email");
      options.Scope.Add("profile");

      options.TokenValidationParameters.NameClaimType = "cognito:username";
      options.TokenValidationParameters.RoleClaimType = "cognito:groups";
  });

builder.Services.AddAuthorization();

//
// =====================
// AI Services
// =====================
builder.Services.AddSingleton<Client>(_ => new Client());
builder.Services.AddScoped<AIReportingService>();

// var googleKey =
//     Environment.GetEnvironmentVariable("GOOGLE_API_KEY")
//     ?? builder.Configuration["Google:ApiKey"]; // optional appsettings slot

// if (!string.IsNullOrWhiteSpace(googleKey))
// {
//     builder.Services.AddSingleton(_ => new Google.GenAI.Client(apiKey: googleKey));
//     builder.Services.AddScoped<AIReportingService>();
//     Console.WriteLine("✅ AI enabled (GOOGLE_API_KEY found).");
// }
// else
// {
//     Console.WriteLine("⚠️ GOOGLE_API_KEY not set; AI features disabled.");
//     // Do NOT register AIReportingService at all.
// }


//
// =====================
// Database
// =====================
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(conn))
{
    throw new InvalidOperationException(
        "Missing connection string 'DefaultConnection'");
}

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseMySql(
        conn,
        new MySqlServerVersion(new Version(8, 0, 36)),
        mySqlOpts => mySqlOpts.EnableRetryOnFailure()
    )
);

//
// =====================
// Application Services
// =====================

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserContextService, UserContextService>();

builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonCognitoIdentityProvider>();
builder.Services.AddScoped<CognitoProvisioningService>();

builder.Services.AddScoped<RecipeService>();
builder.Services.AddScoped<UnitService>();
builder.Services.AddScoped<IngredientService>();
builder.Services.AddScoped<IIngredientService, IngredientService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<LocationState>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IUnitService, UnitService>();
builder.Services.AddScoped<IInventoryTransactionService, InventoryTransactionService>();
builder.Services.AddScoped<IInventoryManagementService, InventoryManagementService>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<ITaskAssignmentService, TaskAssignmentService>();
builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
builder.Services.AddSingleton<EnumService>();

//
// =====================
// Build App
// =====================
var app = builder.Build();

// Determine whether the app should only run migrations and exit
var migrateOnly = (Environment.GetEnvironmentVariable("MIGRATE_ONLY")?.Equals("true", StringComparison.OrdinalIgnoreCase) == true)
                  || (args != null && args.Any(a => a.Equals("--migrate-only", StringComparison.OrdinalIgnoreCase)));

//
// =====================
// Apply pending EF Core migrations at startup
// =====================
using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[Startup] Migration warning: {ex.Message}");
    }
}

//
// =====================
// Middleware
// =====================
if (migrateOnly)
{
    Console.WriteLine("[Startup] MIGRATE_ONLY set; exiting after applying migrations.");
    return;
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    // app.UseHttpsRedirection();
}

// Temporarily disable HTTPS redirect for development

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();
  
//
// =====================
// Routes
// =====================
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapGet("/health", () => "OK");

var cognitoDomain = builder.Configuration["Authentication:Cognito:Domain"];
var clientId = builder.Configuration["Authentication:Cognito:ClientId"];
var callbackPath = builder.Configuration["Authentication:Cognito:CallbackPath"] ?? "/signin-oidc";
var logoutCallbackPath = builder.Configuration["Authentication:Cognito:SignedOutCallbackPath"] ?? "/signout-callback-oidc";

app.MapGet("/login", async (HttpContext ctx) =>
{
    await ctx.ChallengeAsync(
        OpenIdConnectDefaults.AuthenticationScheme,
        new AuthenticationProperties { RedirectUri = "/post-login" }
    );
});

app.MapGet("/logout", async (HttpContext ctx, IConfiguration config) =>
{
    // Clear local cookie
    await ctx.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

    var domain = config["Authentication:Cognito:Domain"]!.TrimEnd('/');
    var clientId = config["Authentication:Cognito:ClientId"]!;

    var postLogout = $"{ctx.Request.Scheme}://{ctx.Request.Host}/"; // must match allowed sign-out URL

    var url = $"{domain}/logout" +
              $"?client_id={Uri.EscapeDataString(clientId)}" +
              $"&logout_uri={Uri.EscapeDataString(postLogout)}";

    return Results.Redirect(url);
});



app.Run();
