using System.Text.Json;
using CulinaryCommand.Data;
using CulinaryCommand.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;

namespace CulinaryCommand.Services
{
    public class AuthService
    {
        private readonly IJSRuntime _js;
        private readonly AppDbContext _context;
        private bool _hydrated;

        private readonly IServiceScopeFactory _scopeFactory;

        public AuthService(IJSRuntime js, IServiceScopeFactory scopeFactory)
        {
            _js = js;
            _scopeFactory = scopeFactory;

            ManagedLocations = new List<Location>();
            Locations = new List<string>();
        }

        public bool IsSignedIn { get; private set; }
        public User? CurrentUser { get; private set; }

        public string? UserEmail { get; private set; }
        public int? UserId { get; private set; }
        public string? UserRole { get; private set; }

        public string? Company { get; private set; }
        public string? CompanyCode { get; private set; }
        public int? CompanyId { get; private set; }

        public string? Location { get; private set; }
        public int? ActiveLocationId { get; private set; }
        public List<int>? LocationIds { get; private set; }

        public List<string> Locations { get; private set; }
        public List<Location> ManagedLocations { get; private set; }

        public event Action? OnAuthStateChanged;
        private void Raise() => OnAuthStateChanged?.Invoke();

        /// <summary>
        /// Ensure the auth state is hydrated from localStorage + DB.
        /// Safe to call multiple times; only runs once.
        /// </summary>
        public async Task EnsureHydratedAsync()
        {
            if (_hydrated) return;

            // Wait for JS/localStorage
            for (int i = 0; i < 50; i++)
            {
                try
                {
                    var test = await _js.InvokeAsync<string?>("localStorage.getItem", "cc_email");
                    if (test != null) break;
                }
                catch { }
                await Task.Delay(50);
            }

            // Create fresh, thread-safe DbContext
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            try
            {
                var idString = await _js.InvokeAsync<string?>("localStorage.getItem", "cc_userId");

                if (!int.TryParse(idString, out var userId))
                {
                    IsSignedIn = false;
                    CurrentUser = null;
                    _hydrated = true;
                    Raise();
                    return;
                }

                var user = await db.Users
                    .Include(u => u.Company)
                    .Include(u => u.UserLocations).ThenInclude(ul => ul.Location)
                    .Include(u => u.ManagerLocations).ThenInclude(ml => ml.Location)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    IsSignedIn = false;
                    CurrentUser = null;
                    _hydrated = true;
                    Raise();
                    return;
                }

                await ApplyUserAsync(user, fromSignIn: false);
                IsSignedIn = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hydration error: {ex}");
                IsSignedIn = false;
                CurrentUser = null;
            }
            finally
            {
                _hydrated = true;
                Raise();
            }
        }

        /// <summary>
        /// Called after a successful login or invite-activation.
        /// </summary>
        public async Task SignInAsync(User user)
        {
            await ApplyUserAsync(user, fromSignIn: true);
            Raise();
        }

        private async Task ApplyUserAsync(User user, bool fromSignIn)
        {
            CurrentUser = user;

            UserId = user.Id;
            UserEmail = user.Email;
            UserRole = user.Role;

            CompanyId = user.CompanyId;
            Company = user.Company?.Name;
            CompanyCode = user.Company?.CompanyCode;

            Locations = user.UserLocations?
                .Select(ul => ul.Location.Name)
                .Distinct()
                .ToList() ?? new List<string>();

            LocationIds = user.UserLocations?
                .Select(ul => ul.LocationId)
                .Distinct()
                .ToList() ?? new List<int>();

            ManagedLocations = user.ManagerLocations
                .Select(ml => ml.Location)
                .ToList();

            // Get active location from localStorage if present
            var activeLocationIdStr = await _js.InvokeAsync<string?>("localStorage.getItem", "cc_activeLocationId");
            if (int.TryParse(activeLocationIdStr, out var activeLocId) && (LocationIds?.Contains(activeLocId) ?? false))
            {
                ActiveLocationId = activeLocId;
                Location = user.UserLocations
                    .Where(ul => ul.LocationId == activeLocId)
                    .Select(ul => ul.Location.Name)
                    .FirstOrDefault();
            }
            else
            {
                ActiveLocationId = LocationIds?.FirstOrDefault();
                Location = user.UserLocations
                    .Select(ul => ul.Location.Name)
                    .FirstOrDefault();
            }

            IsSignedIn = true;

            // Only write to localStorage on sign-in / login-type actions
            if (fromSignIn)
            {
                await _js.InvokeVoidAsync("localStorage.setItem", "cc_userId", UserId?.ToString() ?? "");
                await _js.InvokeVoidAsync("localStorage.setItem", "cc_email", UserEmail ?? "");
                await _js.InvokeVoidAsync("localStorage.setItem", "cc_role", UserRole ?? "");
                await _js.InvokeVoidAsync("localStorage.setItem", "cc_company", Company ?? "");
                await _js.InvokeVoidAsync("localStorage.setItem", "cc_companyCode", CompanyCode ?? "");
                await _js.InvokeVoidAsync("localStorage.setItem", "cc_companyId", CompanyId?.ToString() ?? "");
                await _js.InvokeVoidAsync("localStorage.setItem", "cc_locationIds",
                    JsonSerializer.Serialize(LocationIds ?? new List<int>()));

                await _js.InvokeVoidAsync("localStorage.setItem", "cc_activeLocationId",
                    ActiveLocationId?.ToString() ?? "");
                await _js.InvokeVoidAsync("localStorage.setItem", "cc_location", Location ?? "");
            }
        }

        public async Task LogoutAsync()
        {
            CurrentUser = null;
            UserEmail = null;
            UserId = null;
            UserRole = null;

            Company = null;
            CompanyCode = null;
            CompanyId = null;

            Location = null;
            ActiveLocationId = null;
            LocationIds = null;

            Locations = new List<string>();
            ManagedLocations = new List<Location>();

            IsSignedIn = false;

            await _js.InvokeVoidAsync("localStorage.removeItem", "cc_userId");
            await _js.InvokeVoidAsync("localStorage.removeItem", "cc_email");
            await _js.InvokeVoidAsync("localStorage.removeItem", "cc_role");
            await _js.InvokeVoidAsync("localStorage.removeItem", "cc_company");
            await _js.InvokeVoidAsync("localStorage.removeItem", "cc_companyCode");
            await _js.InvokeVoidAsync("localStorage.removeItem", "cc_companyId");
            await _js.InvokeVoidAsync("localStorage.removeItem", "cc_locationIds");
            await _js.InvokeVoidAsync("localStorage.removeItem", "cc_activeLocationId");
            await _js.InvokeVoidAsync("localStorage.removeItem", "cc_location");

            _hydrated = false;
            Raise();
        }

        public async Task<string?> GetUserIdStringAsync()
        {
            return await _js.InvokeAsync<string?>("localStorage.getItem", "cc_userId");
        }

        public async Task<bool> IsUserSignedInAsync()
        {
            var email = await _js.InvokeAsync<string?>("localStorage.getItem", "cc_email");
            return !string.IsNullOrEmpty(email);
        }

        public async Task UpdateLocation(string newLoc)
        {
            Location = newLoc;
            await _js.InvokeVoidAsync("localStorage.setItem", "cc_location", Location ?? "");
            Raise();
        }

        public async Task UpdateActiveLocation(int newLocId)
        {
            ActiveLocationId = newLocId;
            await _js.InvokeVoidAsync("localStorage.setItem", "cc_activeLocationId", newLocId.ToString());
            Raise();
        }

        public async Task WaitForHydrationAsync()
        {
            if (_hydrated) return;
            await EnsureHydratedAsync();
        }
    }
}