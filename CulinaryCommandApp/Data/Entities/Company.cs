using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Linq;
using System.Threading.Tasks;

namespace CulinaryCommand.Data.Entities
{
    public class Company
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; } = string.Empty;

        // A unique short code used to link employees/locations easily
        [Required]
        [MaxLength(50)]
        public string CompanyCode { get; set; } = string.Empty;

        // Optional details for internal reference
        [MaxLength(256)]
        public string? Address { get; set; }

        [MaxLength(128)]
        public string? City { get; set; }

        [MaxLength(64)]
        public string? State { get; set; }

        [MaxLength(32)]
        public string? ZipCode { get; set; }

        // For contact/admin info
        [MaxLength(128)]
        public string? Phone { get; set; }

        [MaxLength(128)]
        public string? Email { get; set; }

        // Optional descriptive or legal details
        public string? Description { get; set; }
        public string? LLCName { get; set; }
        public string? TaxId { get; set; }

        // Audit
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [JsonIgnore]
        public ICollection<Location>? Locations { get; set; }
        [JsonIgnore]
        public ICollection<User>? Employees { get; set; }
        [JsonIgnore]
        public ICollection<CulinaryCommand.Vendor.Entities.Vendor>? Vendors { get; set; }
    }
}