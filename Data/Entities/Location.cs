using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

                // marginEdgeKey should be nullable, not every location will have one
                public string? MarginEdgeKey { get; set; }

                // keep company nullable until we add a way to create companies
                public int? CompanyId { get; set; }
                public Company? Company { get; set; }

                // join table combining employees and locations
                public ICollection<UserLocation> UserLocations { get; set; } = new List<UserLocation>();

                // join table combining managers and locations
                public ICollection<ManagerLocation> ManagerLocations { get; set; } = new List<ManagerLocation>();


                public ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();

                [NotMapped]
                // so you can still call Location.Managers and get the list
                public IEnumerable<User> Managers => ManagerLocations.Select(ml => ml.User);

                [NotMapped]
                public IEnumerable<User> Employees => UserLocations.Select(ul => ul.User);

        }

}
