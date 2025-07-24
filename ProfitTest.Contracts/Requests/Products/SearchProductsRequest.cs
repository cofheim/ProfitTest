namespace ProfitTest.Contracts.Requests.Products
{
    /// <summary>
    /// запрос на поиск товаров
    /// </summary>
    public record SearchProductsRequest
    {
        public string? NameQuery { get; init; }
        public DateTime? PeriodStart { get; init; }
        public DateTime? PeriodEnd { get; init; }
    }
} 