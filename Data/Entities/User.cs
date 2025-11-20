using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CulinaryCommand.Data.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(256)]
        public string? Name { get; set; }

        // [Required, MaxLength(12)]
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



        // join entities
        [JsonIgnore]
        public ICollection<UserLocation> UserLocations { get; set; } = new List<UserLocation>();
        [JsonIgnore]
        public ICollection<ManagerLocation> ManagerLocations { get; set; } = new List<ManagerLocation>();

        [NotMapped]
        [JsonIgnore]
        public IEnumerable<Location> ManagedLocations => ManagerLocations.Select(ml => ml.Location);
        [NotMapped]
        [JsonIgnore]
        public IEnumerable<Location> Locations => UserLocations.Select(ul => ul.Location);

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
