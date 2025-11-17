// Services/LocationState.cs
using CulinaryCommand.Data.Entities;

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
        public List<Location> ManagedLocations { get; private set; } = new();
        public Location? CurrentLocation { get; private set; }

        public event Action? OnChange;

        public void SetLocations(List<Location> locations)
        {
            ManagedLocations = locations ?? new List<Location>();
            CurrentLocation = ManagedLocations.FirstOrDefault();
            OnChange?.Invoke();
        }

        public void SetCurrentLocation(Location loc)
        {
            CurrentLocation = loc;
            OnChange?.Invoke();
        }
    }
}
