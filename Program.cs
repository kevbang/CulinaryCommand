using Microsoft.EntityFrameworkCore;
using CulinaryCommand.Data;
using CulinaryCommand.Components;
using CulinaryCommand.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//builder.Services.AddRazorPages();

// DB hookup
var conn = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseMySql(conn, new MySqlServerVersion(new Version(8, 0, 23))));


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
//app.UseRouting();
//app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

//app.MapRazorPages();

app.Run();
