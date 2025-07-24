namespace ProfitTest.Contracts.Responses.Auth
{
    /// <summary>
    /// ответ на успешную авторизацию
    /// </summary>
    public record LoginResponse
    {
        public string Token { get; init; } = string.Empty;
        public string UserName { get; init; } = string.Empty;
        public DateTime ExpiresAt { get; init; }
    }
} 