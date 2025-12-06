using System.ComponentModel.DataAnnotations;

namespace CulinaryCommand.Models
{
    public class LocationSignupModel
    {
        [Required, MaxLength(256)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(256)]
        public string Address { get; set; } = string.Empty;

        [Required, MaxLength(256)]
        public string City { get; set; } = string.Empty;

        [Required, MaxLength(256)]
        public string State { get; set; } = string.Empty;

        [Required, MaxLength(256)]
        public string ZipCode { get; set; } = string.Empty;

        public string? MarginEdgeKey { get; set; }
    }
}