using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace EventScheduler.Security
{
    public class PasswordValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var password = value as string;

            if (string.IsNullOrEmpty(password))
            {
                return new ValidationResult("Пароль обязателен.");
            }

            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasDigits = new Regex(@"[0-9]+");
            var hasSpecialChars = new Regex(@"[!@#$%^&*(),.?{}|<>]");


            if (password.Length < 8)
            {
                return new ValidationResult("Пароль должен быть не менее 8 символов.");
            }

            if (!hasUpperChar.IsMatch(password))
            {
                return new ValidationResult("Пароль должен содержать хотя бы одну заглавную букву.");
            }

            if (!hasLowerChar.IsMatch(password))
            {
                return new ValidationResult("Пароль должен содержать хотя бы одну строчную букву.");
            }

            if (!hasDigits.IsMatch(password))
            {
                return new ValidationResult("Пароль должен содержать хотя бы одну цифру.");
            }

            if (!hasSpecialChars.IsMatch(password))
            {
                return new ValidationResult("Пароль должен содержать хотя бы один специальный символ.");
            }

            return ValidationResult.Success;
        }
    }

}
