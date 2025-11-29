using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CulinaryCommand.Data.Entities
{
    public class WorkTask
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(128)]
        public int LocationId { get; set; }

        [Required, MaxLength(128)]
        public int StationId { get; set; }

        [Required, MaxLength(256)]
        public string? Name { get; set; } = string.Empty;

        [Required]
        public string? Description { get; set; } = string.Empty;

        [Required]
        public string? Status { get; set; }

        [Required]
        public int AssignerId { get; set; }

        [Required]
        public DateTime? Date { get; set; }

        // Optional: assign to a user
        [ForeignKey("User")]
        public int? UserId { get; set; }
        public User? User { get; set; }

        public DateTime DueAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}