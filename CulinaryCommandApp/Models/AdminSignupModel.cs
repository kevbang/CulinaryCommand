using System.ComponentModel.DataAnnotations;

namespace CulinaryCommand.Models
{
    public class AdminSignupModel
    {
        [Required, MaxLength(256)]
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(6), MaxLength(256)]
        public string Password { get; set; } = string.Empty;

        [MaxLength(128)]
        public string? Phone { get; set; }
    }
}