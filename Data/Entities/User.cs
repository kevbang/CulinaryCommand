using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CulinaryCommand.Data.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(256)]
        public string? Name { get; set; }

        [Required, MaxLength(12)]
        public string? Phone { get; set; }

        [Required, MaxLength(256)]
        public string? Email { get; set; }

        [Required, MaxLength(256)]
        public string? Password { get; set; }

        [Required, MaxLength(128)]
        public string? Role { get; set; }


        //delete this in the future, shouldn't be needed
        // [Required, MaxLength(128)]
        public int? CompanyId { get; set; }

        public Company? Company { get; set; }

        // Navigation property for UserStation experience
        public string? StationsWorked { get; set; }


        // list of locations this user WORKS at
        public ICollection<Location> Locations { get; set; } = new List<Location>();

        // join entities
        public ICollection<UserLocation> UserLocations { get; set; } = new List<UserLocation>();
        public ICollection<ManagerLocation> ManagerLocations { get; set; } = new List<ManagerLocation>();

        [NotMapped]
        public IEnumerable<Location> ManagedLocations => ManagerLocations.Select(ml => ml.Location);

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
