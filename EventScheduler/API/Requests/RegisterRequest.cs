using EventScheduler.Security;
using System.ComponentModel.DataAnnotations;

namespace EventScheduler.API.Requests
{
    public abstract class RegisterRequest
    {
        [Required(ErrorMessage = "Имя и фамилия обязательны для заполнения.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Электронная почта обязательна.")]
        [EmailAddress(ErrorMessage = "Неверный формат электронной почты.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Только Gmail-адреса разрешены.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль обязателен.")]
        [PasswordValidation(ErrorMessage = "Пароль должен быть не менее 8 символов и содержать заглавные, строчные буквы, цифры и специальные символы.")]
        public string Password { get; set; }
    }


    public class ManagerRegisterRequest : RegisterRequest
    {
        public Guid idCompany { get; set; }
    }

    public class StudentRegisterRequest : RegisterRequest
    {

    }
}
