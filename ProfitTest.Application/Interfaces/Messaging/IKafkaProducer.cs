namespace ProfitTest.Application.Interfaces.Messaging
{
    public interface IKafkaProducer<TMessage> : IDisposable
    {
        Task ProduceAsync(TMessage message, CancellationToken cancellationToken);
    }
}
