namespace ProfitTest.Contracts.Common
{
    /// <summary>
    /// ответ с ошибкой
    /// </summary>
    public record ErrorResponse(string Message, string? Details = null);
} 