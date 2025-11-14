using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CulinaryCommand.Data.Entities
{
    public class Location
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(256)]
        public string? Name { get; set; }

        [Required, MaxLength(256)]
        public string? Address { get; set; }

        [Required, MaxLength(256)]
        public string? City { get; set; }

        [Required, MaxLength(256)]
        public string? State { get; set; }

        [Required, MaxLength(256)]
        public string? ZipCode { get; set; }

        [Required, MaxLength(124)]
        public string? MarginEdgeKey { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        // Relationships
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}

