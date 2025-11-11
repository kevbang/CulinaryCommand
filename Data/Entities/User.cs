using System.ComponentModel.DataAnnotations;

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

        [Required, MaxLength(128)]
        public string? Location { get; set; }

        public int? CompanyId { get; set; }

        public Company? Company { get; set; }

        // Navigation property for UserStation experience
        public string? StationsWorked { get; set; }

        public ICollection<Location> Locations { get; set; } = new List<Location>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}


