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

        /* list of users that WORK at this location
         *
         * many to many relationship, many users can work at multiple locations
         */
        public ICollection<User> Users { get; set; } = new List<User>();

        /**
         * list of users that MANAGE this location
         *
         * right now, i have this as a many-to-many relationship between 
         * locations and managers, meaning each locations can have multiple managers
         */
        public ICollection<User> Managers { get; set; } = new List<User>();

    }
}

