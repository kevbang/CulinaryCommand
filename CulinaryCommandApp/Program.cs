using Microsoft.EntityFrameworkCore;
using CulinaryCommand.Data;
using CulinaryCommand.Services;
using CulinaryCommand.Inventory.Services;
using CulinaryCommand.PurchaseOrder.Services;
using CulinaryCommand.Inventory;
using CulinaryCommand.Inventory.Services.Interfaces;
using System; // for Version, TimeSpan
using System.Linq;
using CulinaryCommand.Components; // for args.Any
using Google.GenAI;
using CulinaryCommandApp.AIDashboard.Services.Reporting;


var builder = WebApplication.CreateBuilder(args);


// FORCE EF to load your config when running "dotnet ef"
// builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
// builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register Google GenAI client and AIReportingService so they can be injected.
// The Client will pick up the GOOGLE_API_KEY from environment variables (set in deploy.yml).
builder.Services.AddSingleton<Client>(_ => new Client());
builder.Services.AddScoped<AIReportingService>();

// DB hookup
// var conn = builder.Configuration.GetConnectionString("DefaultConnection");
// if (string.IsNullOrWhiteSpace(conn))
// {
//     throw new InvalidOperationException("Missing connection string 'Default'. Set ConnectionStrings__Default via environment or config.");
// }
// Console.WriteLine("CONNECTION STRING FROM CONFIG:");
// Console.WriteLine(conn);


// builder.Services.AddDbContext<AppDbContext>(opt =>
//     opt.UseMySql(conn, new MySqlServerVersion(new Version(8, 0, 36)),
//         mySqlOptions => mySqlOptions.EnableRetryOnFailure()
//     )
// );

// DB hookup
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(conn))
{
    throw new InvalidOperationException("Missing connection string 'DefaultConnection'. Set ConnectionStrings__DefaultConnection via environment or config.");
}

// Mask password for logs (primarily for debugging in the Lightsail instance)
string MaskPwd(string s)
{
    if (string.IsNullOrEmpty(s)) return s;
    var parts = s.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    for (int i = 0; i < parts.Length; i++)
    {
        if (parts[i].StartsWith("Pwd=", StringComparison.OrdinalIgnoreCase))
            parts[i] = "Pwd=****";
    }
    return string.Join(';', parts);
}
Console.WriteLine($"[Startup] Using MySQL connection: {MaskPwd(conn)}");

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseMySql(conn, new MySqlServerVersion(new Version(8, 0, 36)), mySqlOpts =>
    {
        // Enable transient retry for RDS connectivity hiccups
        mySqlOpts.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null);
    }));

// registers services with Scoped lifetime.
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<RecipeService>();
builder.Services.AddScoped<UnitService>();
builder.Services.AddScoped<IngredientService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<LocationState>();
builder.Services.AddScoped<IIngredientService, IngredientService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IUnitService, UnitService>();
builder.Services.AddScoped<IInventoryTransactionService, InventoryTransactionService>();
builder.Services.AddScoped<IInventoryManagementService, InventoryManagementService>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<ITaskAssignmentService, TaskAssignmentService>();
builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
builder.Services.AddSingleton<EnumService>();

var app = builder.Build();

// Determine whether the app should only run migrations and exit
var migrateOnly = (Environment.GetEnvironmentVariable("MIGRATE_ONLY")?.Equals("true", StringComparison.OrdinalIgnoreCase) == true)
                  || (args != null && args.Any(a => a.Equals("--migrate-only", StringComparison.OrdinalIgnoreCase)));

// Apply pending EF Core migrations at startup
using (var scope = app.Services.CreateScope())
{
    try
    {
        var database = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        database.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[Startup] Migration failed: {ex.GetType().Name} - {ex.Message}");
        // Optionally: keep running without schema update; remove this catch to fail hard instead
    }
}

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
    app.UseHttpsRedirection();
}

// Temporarily disable HTTPS redirect for development
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
