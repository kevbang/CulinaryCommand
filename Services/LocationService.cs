using System.Text.Json;
using CulinaryCommand.Data;
using CulinaryCommand.Data.Entities;
using Microsoft.AspNetCore.Components;
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

        Task LoadAndPersistLocationsAsync(int userId);

    }

    public class LocationService : ILocationService
    {
        private readonly AppDbContext _context;

        private LocationState _locationState;
        private readonly IJSRuntime _js;

        private readonly AuthService _auth;


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

        public async Task<Location?> CreateLocationAsync(Location location, int creatorUserId)
        {
            // Load the user so we know their company
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == creatorUserId);

            if (user == null)
            {
                return null;
            }

            // Tie the location to the user's company
            if (user.CompanyId.HasValue)
            {
                location.CompanyId = user.CompanyId.Value;
            }

            // Create the location
            _context.Locations.Add(location);
            await _context.SaveChangesAsync();

            // Ensure the creator is assigned to this location as an employee
            var link = new UserLocation
            {
                UserId = creatorUserId,
                LocationId = location.Id
            };
            _context.UserLocations.Add(link);
            await _context.SaveChangesAsync();

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

        public async Task LoadAndPersistLocationsAsync(int userId)
        {
            var locations = await GetAccessibleLocationsForUserAsync(userId);

            var locationIds = locations.Select(l => l.Id).ToList();
            var activeLocationId = locationIds.FirstOrDefault();
            var activeLocationName = locations.FirstOrDefault()?.Name ?? "";

            // Store full list for LocationSelector
            var json = JsonSerializer.Serialize(locations);
            await _js.InvokeVoidAsync("localStorage.setItem", "cc_locations", json);

            // Use AuthService helper to sync ids + active location
            await _auth.UpdateLocationsAsync(locationIds, activeLocationId, activeLocationName);
        }

        public async Task<List<Location>> GetAccessibleLocationsForUserAsync(int userId)
        {
            var user = await _context.Users
                .Include(u => u.Company)
                .Include(u => u.UserLocations)
                    .ThenInclude(ul => ul.Location)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return new List<Location>();

            // If they don’t even have a company, they see nothing
            if (!user.CompanyId.HasValue)
                return new List<Location>();

            // Admins: all locations for their company
            if (string.Equals(user.Role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                return await _context.Locations
                    .Where(l => l.CompanyId == user.CompanyId)
                    .OrderBy(l => l.Name)
                    .ToListAsync();
            }

            // Regular employees: only locations they are assigned to
            return user.UserLocations
                .Where(ul => ul.Location.CompanyId == user.CompanyId)
                .Select(ul => ul.Location)
                .Distinct()
                .OrderBy(l => l.Name)
                .ToList();
        }
    }
}

