using System.ComponentModel.DataAnnotations;

namespace CulinaryCommand.Models
{
    public class InviteUserVm
    {
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; } = "";

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; } = "";

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email.")]
        public string Email { get; set; } = "";

        [Required]
        public string Role { get; set; } = "Employee";

        [MinLength(1, ErrorMessage = "Select at least one location.")]
        public List<int> LocationIds { get; set; } = new();
    }
}