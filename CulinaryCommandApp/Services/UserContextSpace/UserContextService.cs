using System.Security.Claims;
using CulinaryCommand.Data;
using CulinaryCommand.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CulinaryCommand.Services.UserContextSpace;

public sealed class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _http;
    private readonly IServiceScopeFactory _scopeFactory;

    private UserContext? _cached;

    public UserContextService(IHttpContextAccessor http, IServiceScopeFactory scopeFactory)
    {
        _http = http;
        _scopeFactory = scopeFactory;
    }

    public Task<UserContext> GetAsync(CancellationToken ct = default)
        => _cached is not null ? Task.FromResult(_cached) : RefreshAsync(ct);

    public async Task<UserContext> RefreshAsync(CancellationToken ct = default)
    {
        var principal = _http.HttpContext?.User;
        var isAuth = principal?.Identity?.IsAuthenticated == true;

        if (!isAuth || principal is null)
        {
            _cached = new UserContext { IsAuthenticated = false };
            return _cached;
        }

        var email =
            principal.FindFirstValue(ClaimTypes.Email) ??
            principal.FindFirstValue("email");

        var sub =
            principal.FindFirstValue(ClaimTypes.NameIdentifier) ??
            principal.FindFirstValue("sub");

        if (string.IsNullOrWhiteSpace(email))
        {
            _cached = new UserContext { IsAuthenticated = true, Email = null, CognitoSub = sub, User = null };
            return _cached;
        }

        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var user = await db.Users
            .Include(u => u.Company)
                .ThenInclude(c => c.Locations)
            .Include(u => u.UserLocations)
                .ThenInclude(ul => ul.Location)
            .Include(u => u.ManagerLocations)
                .ThenInclude(ml => ml.Location)
            .FirstOrDefaultAsync(u => u.Email == email, ct);

        if (user is null)
        {
            _cached = new UserContext
            {
                IsAuthenticated = true,
                Email = email,
                CognitoSub = sub,
                User = null
            };
            return _cached;
        }

        List<Location> accessibleLocations;

        if (user.Role == "Admin")
        {
            accessibleLocations = user.Company?.Locations?.ToList() ?? new();
        }
        else if (user.Role == "Manager")
        {
            accessibleLocations = user.ManagerLocations?
                .Select(x => x.Location)
                .Distinct()
                .ToList()
                ?? new();

            if (accessibleLocations.Count == 0)
            {
                accessibleLocations = user.UserLocations?
                    .Select(x => x.Location)
                    .Distinct()
                    .ToList()
                    ?? new();
            }
        }
        else
        {
            accessibleLocations = user.UserLocations?
                .Select(x => x.Location)
                .Distinct()
                .ToList()
                ?? new();
        }

        _cached = new UserContext
        {
            IsAuthenticated = true,
            Email = email,
            CognitoSub = sub,
            User = user,
            AccessibleLocations = accessibleLocations
        };

        return _cached;
    }
}
