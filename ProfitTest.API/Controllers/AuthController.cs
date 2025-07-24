using Microsoft.AspNetCore.Mvc;
using ProfitTest.Application.Interfaces.Auth;

namespace ProfitTest.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }
    }
}
