using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProfitTest.Application.Authentication;
using ProfitTest.Application.Interfaces;
using ProfitTest.Application.Interfaces.Auth;
using ProfitTest.Contracts.Responses.Auth;
using ProfitTest.Domain.Models;
using static BCrypt.Net.BCrypt;

namespace ProfitTest.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;
        private readonly ILogger<AuthService> _logger;
        private readonly AuthSettings _authSettings;

        public AuthService(
            IUserRepository userRepository,
            JwtService jwtService,
            IOptions<AuthSettings> authSettings,
            ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _authSettings = authSettings.Value;
            _logger = logger;
        }

        public async Task<(bool Success, LoginResponse? Response, string Error)> LoginAsync(
            string userName,
            string password)
        {
            try
            {
                // получаем пользователя
                var user = await _userRepository.GetByUsernameAsync(userName);
                if (user == null)
                    return (false, null, "Неверное имя пользователя или пароль");

                // проверяем пароль
                if (!Verify(password, user.PasswordHash))
                    return (false, null, "Неверное имя пользователя или пароль");

                // обновляем дату последнего входа
                user.UpdateLastLogin();
                await _userRepository.UpdateAsync(user);

                // создаем JWT токен
                var token = _jwtService.GenerateToken(user);

                var response = new LoginResponse
                {
                    Token = token,
                    UserName = user.UserName,
                    ExpiresAt = DateTime.UtcNow.Add(_authSettings.Expires)
                };

                return (true, response, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при попытке входа пользователя {UserName}", userName);
                return (false, null, "Произошла ошибка при попытке входа");
            }
        }

        public async Task<(bool Success, LoginResponse? Response, string Error)> RegisterAsync(
            string userName,
            string password)
        {
            try
            {
                // проверяем, не занято ли имя пользователя
                var existingUser = await _userRepository.GetByUsernameAsync(userName);
                if (existingUser != null)
                    return (false, null, "Пользователь с таким именем уже существует");

                // хешируем пароль
                var passwordHash = HashPassword(password);

                // создаем пользователя
                var (user, error) = User.Create(userName, passwordHash);
                if (user == null)
                    return (false, null, error);

                // сохраняем пользователя
                await _userRepository.AddAsync(user);

                // создаем JWT токен
                var token = _jwtService.GenerateToken(user);

                var response = new LoginResponse
                {
                    Token = token,
                    UserName = user.UserName,
                    ExpiresAt = DateTime.UtcNow.Add(_authSettings.Expires)
                };

                return (true, response, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при регистрации пользователя {UserName}", userName);
                return (false, null, "Произошла ошибка при регистрации");
            }
        }
    }
}
