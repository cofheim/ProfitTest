namespace ProfitTest.Contracts.Messages
{
    // ��������� ��� ���������� ������
    public record ProductUpdatedMessage(
        Guid Id,
        string Name,
        decimal Price,
        DateTime PriceValidFrom,
        DateTime? PriceValidTo
    );
}
