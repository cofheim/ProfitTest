using System.ComponentModel.DataAnnotations;

namespace ProfitTest.Contracts.Requests.Auth
{
    /// <summary>
    /// запрос на авторизацию
    /// </summary>
    public record LoginRequest
    {
        [Required(ErrorMessage = "Имя пользователя обязательно")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Длина имени пользователя должна быть от 3 до 25 символов")]
        public string UserName { get; init; } = string.Empty;

        [Required(ErrorMessage = "Пароль обязателен")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Длина пароля должна быть от 6 до 50 символов")]
        public string Password { get; init; } = string.Empty;
    }
} 