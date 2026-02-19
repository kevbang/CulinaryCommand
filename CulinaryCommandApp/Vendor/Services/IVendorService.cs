using VE = CulinaryCommand.Vendor.Entities;

namespace CulinaryCommand.Vendor.Services
{
    public interface IVendorService
    {
        /// <summary>Get all vendors belonging to a company.</summary>
        Task<List<VE.Vendor>> GetVendorsByCompanyAsync(int companyId);

        /// <summary>Get vendors currently assigned to a specific location.</summary>
        Task<List<VE.Vendor>> GetVendorsByLocationAsync(int locationId);

        /// <summary>Create a new vendor for a company.</summary>
        Task<VE.Vendor> CreateVendorAsync(VE.Vendor vendor);

        /// <summary>Update an existing vendor.</summary>
        Task UpdateVendorAsync(VE.Vendor vendor);

        /// <summary>Delete a vendor by ID.</summary>
        Task DeleteVendorAsync(int vendorId);

        /// <summary>Assign a vendor to a location.</summary>
        Task AddVendorToLocationAsync(int locationId, int vendorId);

        /// <summary>Remove a vendor from a location.</summary>
        Task RemoveVendorFromLocationAsync(int locationId, int vendorId);

        /// <summary>
        /// Set the complete list of vendors for a location,
        /// adding/removing as needed to match the provided IDs.
        /// </summary>
        Task SetLocationVendorsAsync(int locationId, IEnumerable<int> vendorIds);
    }
}
