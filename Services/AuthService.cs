using CulinaryCommand.Data.Entities;
using Microsoft.JSInterop;

namespace CulinaryCommand.Services
{
    public class AuthService
    {
        private readonly IJSRuntime _js;
        private bool _hydrated;

        public AuthService(IJSRuntime js) { _js = js; }

        public bool IsSignedIn { get; private set; }
        public string? CurrentUser { get; private set; }
        public string? UserRole { get; private set; }
        public string? Company { get; private set; }
        public string? CompanyCode { get; private set; }
        public string? Location { get; private set; }

        public event Action? OnAuthStateChanged;
        private void Raise() => OnAuthStateChanged?.Invoke();

        public List<string>? Locations { get; set; }

        public void NotifyStateChanged() => OnAuthStateChanged?.Invoke();


        public async Task InitializeFromJsAsync()
        {
            if (_hydrated) return;
            try
            {
                CurrentUser = await _js.InvokeAsync<string?>("localStorage.getItem", "cc_user");
                UserRole = await _js.InvokeAsync<string?>("localStorage.getItem", "cc_role");
                Company = await _js.InvokeAsync<string?>("localStorage.getItem", "cc_company");
                CompanyCode = await _js.InvokeAsync<string?>("localStorage.getItem", "cc_companyCode");
                Location = await _js.InvokeAsync<string?>("localStorage.getItem", "cc_location");

                IsSignedIn = !string.IsNullOrEmpty(CurrentUser);
            }
            catch
            {
                // ignore; treat as signed out
                IsSignedIn = false;
            }
            finally
            {
                _hydrated = true;
                Raise();
            }
        }

        public async Task SignInAsync(User user)
        {
            CurrentUser = user.Email;
            UserRole = user.Role;
            Company = user.Company?.Name;
            CompanyCode = user.Company?.CompanyCode;
            Locations = user.Locations?.Select(l => l.Name).ToList();
            Location = user.Locations.FirstOrDefault()?.Name;
            IsSignedIn = true;

            await _js.InvokeVoidAsync("localStorage.setItem", "cc_user", CurrentUser ?? "");
            await _js.InvokeVoidAsync("localStorage.setItem", "cc_role", UserRole ?? "");
            await _js.InvokeVoidAsync("localStorage.setItem", "cc_company", Company ?? "");
            await _js.InvokeVoidAsync("localStorage.setItem", "cc_companyCode", CompanyCode ?? "");
            await _js.InvokeVoidAsync("localStorage.setItem", "cc_location", Location ?? "");
            Raise();
        }

        public async Task LogoutAsync()
        {
            CurrentUser = null;
            UserRole = Company = CompanyCode = Location = null;
            IsSignedIn = false;

            await _js.InvokeVoidAsync("localStorage.removeItem", "cc_user");
            await _js.InvokeVoidAsync("localStorage.removeItem", "cc_role");
            await _js.InvokeVoidAsync("localStorage.removeItem", "cc_company");
            await _js.InvokeVoidAsync("localStorage.removeItem", "cc_companyCode");
            await _js.InvokeVoidAsync("localStorage.removeItem", "cc_location");
            Raise();
        }

        public async Task<string?> GetUser()
        {
            return await _js.InvokeAsync<string?>("localStorage.getItem", "cc_user");
        }

        public async Task<bool> IsUserSignedIn()
        {
            string? User = await _js.InvokeAsync<string?>("localStorage.getItem", "cc_user");
            return User != null ? true : false;
        }

        public async Task UpdateLocation(string newLoc)
        {
            this.Location = newLoc;
            await _js.InvokeVoidAsync("localStorage.setItem", "cc_location", this.Location ?? "");
        }

    }
}