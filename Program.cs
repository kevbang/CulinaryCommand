using Microsoft.EntityFrameworkCore;
using CulinaryCommand.Data;
using CulinaryCommand.Components;
using CulinaryCommand.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// DB hookup
var conn = builder.Configuration.GetConnectionString("Default");
if (string.IsNullOrWhiteSpace(conn))
{
    throw new InvalidOperationException("Missing connection string 'Default'. Set ConnectionStrings__Default via environment or config.");
}

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseMySql(conn, ServerVersion.AutoDetect(conn)));

// registers a service with ASP.NET Core's dependency injection (DI) container using the Scoped lifetime.
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<AuthService>();

var app = builder.Build();

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
