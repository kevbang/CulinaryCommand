// Services/LocationState.cs
using CulinaryCommand.Data.Entities;
using Microsoft.JSInterop;

namespace CulinaryCommand.Services
{
    public class LocationState
    {
        private readonly IJSRuntime _js;

        public List<Location> ManagedLocations { get; private set; } = new();
        public Location? CurrentLocation { get; private set; }

        public event Action? OnChange;

        public LocationState(IJSRuntime js)
        {
            _js = js;
        }

        public async Task SetLocationsAsync(List<Location>? locations)
        {
            ManagedLocations = locations ?? new List<Location>();

            // Try to restore saved current location (but JS may not be ready during prerender)
            string? savedId = null;
            try
            {
                savedId = await _js.InvokeAsync<string?>("localStorage.getItem", "cc_activeLocationId");
            }
            catch (InvalidOperationException)
            {
                // JS not available yet (prerender). We'll just pick a default for now.
            }

            if (int.TryParse(savedId, out var id))
            {
                CurrentLocation = ManagedLocations.FirstOrDefault(l => l.Id == id);
            }

            // Default to first if invalid or not found
            CurrentLocation ??= ManagedLocations.FirstOrDefault();

            OnChange?.Invoke();
        }

        public async Task SetCurrentLocationAsync(Location? loc)
        {
            CurrentLocation = loc;

            try
            {
                if (loc is null)
                    await _js.InvokeVoidAsync("localStorage.removeItem", "cc_activeLocationId");
                else
                    await _js.InvokeVoidAsync("localStorage.setItem", "cc_activeLocationId", loc.Id.ToString());
            }
            catch (InvalidOperationException)
            {
                // JS not available yet (prerender). It's fine; UI state still updates.
            }

            OnChange?.Invoke();
        }
    }
}
