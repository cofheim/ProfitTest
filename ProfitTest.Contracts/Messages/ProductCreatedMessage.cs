namespace ProfitTest.Contracts.Messages
{
    // сообщение для создания товара
    public record ProductCreatedMessage(string Name,
        decimal Price,
        DateTime PriceValidFrom,
        DateTime? PriceValidTo);
}
