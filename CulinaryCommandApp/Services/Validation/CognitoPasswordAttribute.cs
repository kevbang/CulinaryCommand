using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CulinaryCommand.Services
{
    public sealed class CognitoPasswordAttribute : ValidationAttribute
    {
        public int MinLength { get; init; } = 8;
        public bool RequireUppercase { get; init; } = true;
        public bool RequireLowercase { get; init; } = true;
        public bool RequireNumber { get; init; } = true;
        public bool RequireSymbol { get; init; } = true;

        // Cognito “special characters” list from AWS docs.
        // If your pool requires symbols, Cognito checks against this set. :contentReference[oaicite:2]{index=2}
        private const string CognitoSymbols = "^$*.[\\]{}()?\"!@#%&/\\\\,><':;|_~`=+-";

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var s = value as string ?? "";
            var errors = new StringBuilder();

            if (s.Length < MinLength)
                errors.AppendLine($"Password must be at least {MinLength} characters.");

            if (RequireUppercase && !s.Any(char.IsUpper))
                errors.AppendLine("Password must contain at least 1 uppercase letter.");

            if (RequireLowercase && !s.Any(char.IsLower))
                errors.AppendLine("Password must contain at least 1 lowercase letter.");

            if (RequireNumber && !s.Any(char.IsDigit))
                errors.AppendLine("Password must contain at least 1 number.");

            if (RequireSymbol)
            {
                // Match Cognito's allowed symbol set.
                if (!s.Any(c => CognitoSymbols.Contains(c)))
                    errors.AppendLine("Password must contain at least 1 special character.");
            }

            return errors.Length == 0
                ? ValidationResult.Success
                : new ValidationResult(errors.ToString().Trim());
        }
    }    
}

