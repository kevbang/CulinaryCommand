using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CulinaryCommand.Data.Entities
{
    public class Location
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(256)]
        public string Name { get; set; }

        [Required, MaxLength(256)]
        public string Address { get; set; }

        [Required, MaxLength(256)]
        public string City { get; set; }

        [Required, MaxLength(256)]
        public string State { get; set; }

        [Required, MaxLength(256)]
        public string ZipCode { get; set; }

        public string? MarginEdgeKey { get; set; }

        // Foreign key to company
        public int CompanyId { get; set; }

        [JsonIgnore]
        public Company? Company { get; set; }

        // Recipes owned by this location
        public ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();

        // Many-to-many: users assigned to this location
        [JsonIgnore]
        public ICollection<UserLocation> UserLocations { get; set; } = new List<UserLocation>();

        [NotMapped]
        public IEnumerable<User> Employees => UserLocations.Select(ul => ul.User);
    }
}
