using ProfitTest.Contracts.Responses.Auth;

namespace ProfitTest.Application.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<(bool Success, LoginResponse? Response, string Error)> LoginAsync(string userName, string password);

        Task<(bool Success, LoginResponse? Response, string Error)> RegisterAsync(string userName, string password);
    }
}
