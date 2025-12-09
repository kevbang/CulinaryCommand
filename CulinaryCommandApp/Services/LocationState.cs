// Services/LocationState.cs
using CulinaryCommand.Data.Entities;
using Microsoft.JSInterop;

namespace CulinaryCommand.Services
{
    /*
     * This class holds the current location state of 
     * the app, including the list of managed locations
     * of the logged in user
     *
     *
     *
     */
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

        public async Task SetLocationsAsync(List<Location> locations)
        {
            ManagedLocations = locations ?? new List<Location>();

            // Try to restore saved current location
            var savedId = await _js.InvokeAsync<string?>("localStorage.getItem", "cc_activeLocationId");

            if (int.TryParse(savedId, out int id))
            {
                CurrentLocation = ManagedLocations.FirstOrDefault(l => l.Id == id);
            }

            // Default to first if invalid
            CurrentLocation ??= ManagedLocations.FirstOrDefault();

            OnChange?.Invoke();
        }

        public async Task SetCurrentLocationAsync(Location loc)
        {
            CurrentLocation = loc;

            // Persist to localStorage
            await _js.InvokeVoidAsync("localStorage.setItem", "cc_activeLocationId", loc.Id);

            OnChange?.Invoke();
        }
    }

}
