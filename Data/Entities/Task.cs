using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entities
{
    public class Task
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(256)]
        public string? Name { get; set; } = string.Empty;

        [Required, MaxLength(128)]
        public String? Station { get; set; } = string.Empty;

        [Required]
        public String? Status { get; set; }

        [Required]
        public String? Assigner { get; set; }

        [Required]
        public DateTime? Date { get; set; }

        // Optional: assign to a user
        [ForeignKey("User")]
        public int? UserId { get; set; }
        public User? User { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}