using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Location
    {
        [Key]
        public int ID { get; set; }

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

        // Relationships
        public ICollection<User> Users { get; set; } = new List<User>();
    }

}