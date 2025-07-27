using System.ComponentModel.DataAnnotations;

namespace ProfitTest.Contracts.Requests.Auth
{
    /// <summary>
    /// запрос на регистрацию
    /// </summary>
    public class RegisterRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
} 