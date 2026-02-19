using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CulinaryCommand.Data.Entities;

namespace CulinaryCommand.Vendor.Entities
{
    public class Vendor
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(256)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(256)]
        public string? ContactName { get; set; }

        [MaxLength(256)]
        public string? Email { get; set; }

        [MaxLength(50)]
        public string? Phone { get; set; }

        [MaxLength(256)]
        public string? Website { get; set; }

        [MaxLength(512)]
        public string? LogoUrl { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; } = default!;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public ICollection<LocationVendor> LocationVendors { get; set; } = new List<LocationVendor>();
    }
}
