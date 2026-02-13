using System.ComponentModel.DataAnnotations;

namespace CulinaryCommand.Models
{
    public class SignupRequest
    {
        [Required]
        public CompanySignup Company { get; set; } = new();

        [Required]
        public AdminSignup Admin { get; set; } = new();

        [MinLength(1, ErrorMessage = "At least one location is required.")]
        public List<LocationSignup> Locations { get; set; } = new() { new LocationSignup() };
    }

    public class CompanySignup
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }

        public string? Phone { get; set; }
        public string? Email { get; set; }

        public string? Description { get; set; }
        public string? LLCName { get; set; }
        public string? TaxId { get; set; }
    }

    public class LocationSignup
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string State { get; set; } = string.Empty;

        [Required]
        public string ZipCode { get; set; } = string.Empty;

        public string? MarginEdgeKey { get; set; }
    }

    public class AdminSignup
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Phone { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
