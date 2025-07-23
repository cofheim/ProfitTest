namespace ProfitTest.Contracts.Messages
{
    // сообщение для обновления товара
    public record ProductUpdatedMessage(
        Guid Id,
        string Name,
        decimal Price,
        DateTime PriceValidFrom,
        DateTime? PriceValidTo
    );
}
