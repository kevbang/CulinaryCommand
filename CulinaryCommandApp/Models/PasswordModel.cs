using System.ComponentModel.DataAnnotations;
using CulinaryCommand.Services;
namespace CulinaryCommand.Models
{
    public class PasswordModel
    {
        [Required(ErrorMessage = "Password is required.")]
        [CognitoPassword(
            MinLength = 8,
            RequireUppercase = true,
            RequireLowercase = true,
            RequireNumber = true,
            RequireSymbol = true)]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "Please confirm your password.")]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = "";
    }   
}

