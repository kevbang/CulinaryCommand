using CulinaryCommand.Data.Entities;
using Microsoft.JSInterop;

namespace CulinaryCommand.Services
{
    public class AuthService
    {
        private readonly IJSRuntime _js;
        private bool _hydrated;

        public AuthService(IJSRuntime js) 
        {
            _js = js;
        }

        public bool IsSignedIn { get; private set; }
        public User? CurrentUser { get; private set; }
        public string? UserEmail { get; private set; }
        public int? UserId { get; private set; }
        public string? UserRole { get; private set; }
        public string? Company { get; private set; }
        public string? CompanyCode { get; private set; }

        

        public event Action? OnAuthStateChanged;
        private void Raise() => OnAuthStateChanged?.Invoke();
        public void NotifyStateChanged() => OnAuthStateChanged?.Invoke();


        public async Task InitializeFromJsAsync()
        {
            if (_hydrated) return;
            try
            {
                var idString = await _js.InvokeAsync<string?>("localStorage.getItem", "cc_userId");
                UserId = int.TryParse(idString, out var parsed) ? parsed : null;
                UserEmail = await _js.InvokeAsync<string?>("localStorage.getItem", "cc_email");
                UserRole = await _js.InvokeAsync<string?>("localStorage.getItem", "cc_role");
                Company = await _js.InvokeAsync<string?>("localStorage.getItem", "cc_company");
                CompanyCode = await _js.InvokeAsync<string?>("localStorage.getItem", "cc_companyCode");

                IsSignedIn = !string.IsNullOrEmpty(UserEmail);
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
            CurrentUser = user;
            UserEmail = user.Email;
            UserId = user.Id;
            UserRole = user.Role;
            Company = user.Company?.Name;
            CompanyCode = user.Company?.CompanyCode;
            
            // still need to implement the user methods in LocationService
            // Locations = user.Locations?.Select(l => l.Name).ToList();
            IsSignedIn = true;

            await _js.InvokeVoidAsync("localStorage.setItem", "cc_userId", UserId?.ToString() ?? "");
            await _js.InvokeVoidAsync("localStorage.setItem", "cc_email", UserEmail ?? "");
            await _js.InvokeVoidAsync("localStorage.setItem", "cc_role", UserRole ?? "");
            await _js.InvokeVoidAsync("localStorage.setItem", "cc_company", Company ?? "");
            await _js.InvokeVoidAsync("localStorage.setItem", "cc_companyCode", CompanyCode ?? "");

            Raise();
        }

        public async Task LogoutAsync()
        {
            CurrentUser = null;
            UserEmail = UserRole = Company = CompanyCode = null;
            UserId = null;
            IsSignedIn = false;

            await _js.InvokeVoidAsync("localStorage.removeItem", "cc_userId");

            await _js.InvokeVoidAsync("localStorage.removeItem", "cc_email");
            await _js.InvokeVoidAsync("localStorage.removeItem", "cc_role");
            await _js.InvokeVoidAsync("localStorage.removeItem", "cc_company");
            await _js.InvokeVoidAsync("localStorage.removeItem", "cc_companyCode");
            await _js.InvokeVoidAsync("localStorage.removeItem", "cc_location");
            Raise();
        }

        public async Task<string?> GetUser()
        {
            return await _js.InvokeAsync<string?>("localStorage.getItem", "cc_userId");
        }

        public async Task<bool> IsUserSignedIn()
        {
            string? User = await _js.InvokeAsync<string?>("localStorage.getItem", "cc_email");
            return User != null ? true : false;
        }

    }
}