using CulinaryCommand.Data;
using VE = CulinaryCommand.Vendor.Entities;
using Microsoft.EntityFrameworkCore;

namespace CulinaryCommand.Vendor.Services
{
    public class VendorService : IVendorService
    {
        private readonly AppDbContext _db;

        public VendorService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<VE.Vendor>> GetVendorsByCompanyAsync(int companyId)
        {
            return await _db.Vendors
                .Where(v => v.CompanyId == companyId && v.IsActive)
                .OrderBy(v => v.Name)
                .ToListAsync();
        }

        public async Task<List<VE.Vendor>> GetVendorsByLocationAsync(int locationId)
        {
            return await _db.LocationVendors
                .Where(lv => lv.LocationId == locationId)
                .Include(lv => lv.Vendor)
                .Select(lv => lv.Vendor)
                .OrderBy(v => v.Name)
                .ToListAsync();
        }

        public async Task<VE.Vendor> CreateVendorAsync(VE.Vendor vendor)
        {
            vendor.CreatedAt = DateTime.UtcNow;
            vendor.UpdatedAt = DateTime.UtcNow;
            _db.Vendors.Add(vendor);
            await _db.SaveChangesAsync();
            return vendor;
        }

        public async Task UpdateVendorAsync(VE.Vendor vendor)
        {
            vendor.UpdatedAt = DateTime.UtcNow;
            _db.Vendors.Update(vendor);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteVendorAsync(int vendorId)
        {
            var vendor = await _db.Vendors.FindAsync(vendorId);
            if (vendor is null) return;

            // Soft-delete: keep history intact
            vendor.IsActive = false;
            vendor.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }

        public async Task AddVendorToLocationAsync(int locationId, int vendorId)
        {
            var exists = await _db.LocationVendors
                .AnyAsync(lv => lv.LocationId == locationId && lv.VendorId == vendorId);

            if (!exists)
            {
                _db.LocationVendors.Add(new VE.LocationVendor
                {
                    LocationId = locationId,
                    VendorId = vendorId,
                    AssignedAt = DateTime.UtcNow
                });
                await _db.SaveChangesAsync();
            }
        }

        public async Task RemoveVendorFromLocationAsync(int locationId, int vendorId)
        {
            var link = await _db.LocationVendors
                .FirstOrDefaultAsync(lv => lv.LocationId == locationId && lv.VendorId == vendorId);

            if (link is not null)
            {
                _db.LocationVendors.Remove(link);
                await _db.SaveChangesAsync();
            }
        }

        public async Task SetLocationVendorsAsync(int locationId, IEnumerable<int> vendorIds)
        {
            var desired = vendorIds.ToHashSet();

            var existing = await _db.LocationVendors
                .Where(lv => lv.LocationId == locationId)
                .ToListAsync();

            var existingIds = existing.Select(lv => lv.VendorId).ToHashSet();

            // Remove vendors no longer in the list
            var toRemove = existing.Where(lv => !desired.Contains(lv.VendorId)).ToList();
            _db.LocationVendors.RemoveRange(toRemove);

            // Add new vendors
            foreach (var vendorId in desired.Where(id => !existingIds.Contains(id)))
            {
                _db.LocationVendors.Add(new VE.LocationVendor
                {
                    LocationId = locationId,
                    VendorId = vendorId,
                    AssignedAt = DateTime.UtcNow
                });
            }

            await _db.SaveChangesAsync();
        }
    }
}
