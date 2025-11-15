using Microsoft.EntityFrameworkCore;
using CulinaryCommand.Data;
using CulinaryCommand.Components;
using CulinaryCommand.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// DB hookup
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(conn))
{
    throw new InvalidOperationException("Missing connection string 'Default'. Set ConnectionStrings__Default via environment or config.");
}

builder.Services.AddDbContext<AppDbContext>(opt =>
    // Use an explicit MySQL server version to avoid ServerVersion.AutoDetect opening a connection at design-time
    opt.UseMySql(conn, new MySqlServerVersion(new Version(8, 0, 36))));

// registers a service with ASP.NET Core's dependency injection (DI) container using the Scoped lifetime.
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<AuthService>();

builder.Services.AddScoped<ILocationService, LocationService>();

// for manager state info (using it to store restaurants right now, no interaction with the backend yet)
builder.Services.AddScoped<ManagerSessionManager>();


var app = builder.Build();

// Apply pending EF core migrations at startup
using (var scope = app.Services.CreateScope())
{
    var database = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    database.Database.Migrate();
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
