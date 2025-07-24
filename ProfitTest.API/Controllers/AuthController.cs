using Microsoft.AspNetCore.Mvc;
using ProfitTest.Application.Interfaces.Auth;
using ProfitTest.Contracts.Common;
using ProfitTest.Contracts.Requests.Auth;
using ProfitTest.Contracts.Responses.Auth;

namespace ProfitTest.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var (success, response, error) = await _authService.RegisterAsync(request.UserName, request.Password);

            if (!success)
            {
                _logger.LogError("Ошибка регистрации: {Error}", error);
                return BadRequest(new ErrorResponse(error));
            }

            // устанавливаем JWT токен в куки
            Response.Cookies.Append("myToken", response.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = response.ExpiresAt
            });

            return Ok(response);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var (success, response, error) = await _authService.LoginAsync(
                request.UserName,
                request.Password);

            if (!success)
                return BadRequest(new ErrorResponse(error));

            // устанавливаем JWT токен в куки
            Response.Cookies.Append("myToken", response.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = response.ExpiresAt
            });

            return Ok(response);
        }
    }
}
