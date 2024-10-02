using System.ComponentModel.DataAnnotations;

namespace EventScheduler.API.Requests
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Электронная почта обязательна.")]
        [EmailAddress(ErrorMessage = "Неверный формат электронной почты.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Только Gmail-адреса разрешены.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль обязателен.")]
        public string Password { get; set; }
    }
}
