using Microsoft.JSInterop;

namespace CulinaryCommand.Services
{
    public class AuthService
    {
        private readonly IJSRuntime _js;
        private bool _initialized;

        public AuthService(IJSRuntime js)
        {
            _js = js;
        }

        public bool IsSignedIn { get; private set; }
        public string? CurrentUser { get; private set; }

        public event Action? OnAuthStateChanged;

        public async Task EnsureInitializedAsync()
        {
            if (_initialized)
                return;

            try
            {
                CurrentUser = await _js.InvokeAsync<string?>("localStorage.getItem", "cc_user");
                IsSignedIn = !string.IsNullOrEmpty(CurrentUser);
                _initialized = true;
                NotifyStateChanged();
            }
            catch
            {
                // ignore errors when prerendering (JS not available)
            }
        }

        public async Task SignInAsync(string username)
        {
            CurrentUser = username;
            IsSignedIn = true;
            await _js.InvokeVoidAsync("localStorage.setItem", "cc_user", username);
            NotifyStateChanged();
        }

        public async Task LogoutAsync()
        {
            CurrentUser = null;
            IsSignedIn = false;
            await _js.InvokeVoidAsync("localStorage.removeItem", "cc_user");
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnAuthStateChanged?.Invoke();
    }
}
