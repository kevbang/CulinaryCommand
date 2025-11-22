using Microsoft.EntityFrameworkCore;
using CulinaryCommand.Data;
using CulinaryCommand.Components;
using CulinaryCommand.Services;
using System; // for Version, TimeSpan

var builder = WebApplication.CreateBuilder(args);


// FORCE EF to load your config when running "dotnet ef"
// builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
// builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

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

var app = builder.Build();

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

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Temporarily disable HTTPS redirect for development
//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Simple health endpoint for load balancers/CI checks
app.MapGet("/health", () => "OK");

app.Run();
