using ProfitTest.Contracts.Common;

namespace ProfitTest.Contracts.Responses.Products
{
    /// <summary>
    /// ответ со списком товаров
    /// </summary>
    public record ProductListResponse
    {
        public IReadOnlyList<ProductResponse> Items { get; init; } = new List<ProductResponse>();
        public string? AppliedNameFilter { get; init; }
        public DateTime? AppliedPeriodStart { get; init; }
        public DateTime? AppliedPeriodEnd { get; init; }
    }
} 