using System.Text.Json.Serialization;
using CulinaryCommand.Data.Entities;

namespace CulinaryCommand.Vendor.Entities
{
    /// <summary>
    /// Join table linking a Vendor to a specific Location.
    /// Tracks which vendors are approved/enabled for a given restaurant location.
    /// </summary>
    public class LocationVendor
    {
        public int LocationId { get; set; }

        [JsonIgnore]
        public Location Location { get; set; } = default!;

        public int VendorId { get; set; }

        [JsonIgnore]
        public Vendor Vendor { get; set; } = default!;

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    }
}
