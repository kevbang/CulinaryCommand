using System.Text.Json;
using CulinaryCommand.Data;
using CulinaryCommand.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;


namespace CulinaryCommand.Services
{
    public interface ILocationService
    {

        Task<List<Location>> GetAllLocationsAsync();
        Task<Location?> GetLocationByIdAsync(int id);
        Task<Location?> CreateLocationAsync(Location location, int managerId);
        Task<bool> UpdateLocationAsync(Location location);
        Task<bool> DeleteLocationAsync(int id);

        // User-specific operations (have not implemented yet)

        // Task<bool> AddUserToLocationAsync(int locationId, int userId);
        // Task<bool> RemoveUserFromLocationAsync(int locationId, int userId);
        // Task<List<User>> GetUsersForLocationAsync(int locationId);

        // Manager-specific operations
        Task<bool> AddManagerToLocationAsync(int locationId, int managerId);
        Task<bool> RemoveManagerFromLocationAsync(int locationId, int managerId);
        Task<List<User>> GetManagersForLocationAsync(int locationId);

        Task<List<Location>> GetLocationsByManagerAsync(int? managerId);

        Task<List<Location>> GetLocationsByCompanyAsync(int? companyId);


        Task LoadAndPersistLocationsAsync(int userId);

    }

    public class LocationService : ILocationService
    {
        private readonly AppDbContext _context;

        private LocationState _locationState;
        private readonly IJSRuntime _js;


        public LocationService(AppDbContext context, LocationState locationState, IJSRuntime js)
        {
            _context = context;
            _locationState = locationState;
            _js = js;
        }

        // -------------------- CRUD --------------------
        public async Task<List<Location>> GetAllLocationsAsync()
        {
            return await _context.Locations
                .Include(l => l.Company)
                .Include(l => l.ManagerLocations)
                    .ThenInclude(ml => ml.User)
                .ToListAsync();
        }

        public async Task<Location?> GetLocationByIdAsync(int id)
        {
            return await _context.Locations
                .Include(l => l.Company)
                .Include(l => l.ManagerLocations)
                    .ThenInclude(ml => ml.User)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Location?> CreateLocationAsync(Location location, int managerId)
        {
            // Create the location
            _context.Locations.Add(location);
            await _context.SaveChangesAsync();

            //Add manager to many-to-many link with ManagerLocation
            await AddManagerToLocationAsync(location.Id, managerId);

            //Add user to many-to-many link with UserLocation
            await AddUserToLocationAsync(location.Id, managerId);

            return location;
        }

        public async Task<bool> UpdateLocationAsync(Location location)
        {
            var existing = await _context.Locations.FindAsync(location.Id);
            if (existing == null) return false;


            existing.Name = location.Name;
            existing.Address = location.Address;
            existing.City = location.City;
            existing.State = location.State;
            existing.ZipCode = location.ZipCode;
            existing.MarginEdgeKey = location.MarginEdgeKey;
            existing.CompanyId = location.CompanyId;


            _context.Locations.Update(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteLocationAsync(int id)
        {
            var loc = await _context.Locations.FindAsync(id);
            if (loc == null) return false;

            _context.Locations.Remove(loc);
            await _context.SaveChangesAsync();
            return true;
        }

        // -------------------- Manager Operations --------------------
        public async Task<bool> AddManagerToLocationAsync(int locationId, int managerId)
        {
            var location = await _context.Locations
                .Include(l => l.ManagerLocations)
                .FirstOrDefaultAsync(l => l.Id == locationId);

            var manager = await _context.Users.FindAsync(managerId);

            if (location == null || manager == null) return false;

            // Check if the manager is already linked
            bool alreadyExists = location.ManagerLocations
                .Any(ml => ml.UserId == managerId);

            if (!alreadyExists)
            {
                var managerLocation = new ManagerLocation
                {
                    UserId = managerId,
                    LocationId = locationId,
                    // DateJoined = DateTime.UtcNow
                };

                _context.ManagerLocations.Add(managerLocation);
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> RemoveManagerFromLocationAsync(int locationId, int managerId)
        {
            var managerLocation = await _context.ManagerLocations
                .FirstOrDefaultAsync(ml => ml.LocationId == locationId && ml.UserId == managerId);

            if (managerLocation == null) return false;

            _context.ManagerLocations.Remove(managerLocation);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddUserToLocationAsync(int locationId, int userId)
        {
            var existing = await _context.UserLocations
                .FirstOrDefaultAsync(ul => ul.LocationId == locationId && ul.UserId == userId);

            if (existing != null)
                return true; // Already linked

            var link = new UserLocation
            {
                UserId = userId,
                LocationId = locationId
            };

            _context.UserLocations.Add(link);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<List<User>> GetManagersForLocationAsync(int locationId)
        {
            var location = await _context.Locations
                .Include(l => l.ManagerLocations)
                    .ThenInclude(ml => ml.User)
                .FirstOrDefaultAsync(l => l.Id == locationId);

            return location?.Managers.ToList() ?? new List<User>();
        }

        public async Task<List<Location>> GetLocationsByManagerAsync(int? managerId)
        {
            return await _context.Locations
            .Where(l => l.ManagerLocations.Any(ml => ml.UserId == managerId))
            .Include(l => l.ManagerLocations)
                .ThenInclude(ml => ml.User)
            .ToListAsync();
        }

        public async Task<List<Location>> GetLocationsByCompanyAsync(int? companyId)
        {
            return await _context.Locations
            .Where(l => l.CompanyId == companyId)
            .ToListAsync();
        }

        public async Task LoadAndPersistLocationsAsync(int userId)
        {
            // 1. Get from DB
            var locations = await GetLocationsByManagerAsync(userId);

            // 2. Push into LocationState (in-memory)
            _locationState.SetLocations(locations);

            // 3. Save into localStorage
            var json = JsonSerializer.Serialize(locations);
            await _js.InvokeVoidAsync("localStorage.setItem", "cc_locations", json);

            Location? currentLocation = _locationState.CurrentLocation;
            if (currentLocation != null)
            {
                await _js.InvokeVoidAsync("localStorage.setItem", "cc_activeLocationId", currentLocation.Id);

            }
        }
    }
}

