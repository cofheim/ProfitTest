namespace ProfitTest.Contracts.Responses.Products
{
    /// <summary>
    /// ответ с данными товара
    /// </summary>
    public record ProductResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public DateTime PriceValidFrom { get; init; }
        public DateTime? PriceValidTo { get; init; }
        public DateTime CreatedAt { get; init; }
        public bool IsPriceActive { get; init; }
    }
} 