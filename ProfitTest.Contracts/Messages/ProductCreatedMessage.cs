namespace ProfitTest.Contracts.Messages
{
    // ��������� ��� �������� ������
    public record ProductCreatedMessage(string Name,
        decimal Price,
        DateTime PriceValidFrom,
        DateTime? PriceValidTo);
}
