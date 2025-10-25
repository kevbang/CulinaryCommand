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
        public String? Role { get; set; } = string.Empty;  // Change to ICollection?

        // Navigation property for UserStation experience
        public ICollection<String>? StationsWorked { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<int>? LocationId { get; set; }
    }
}


