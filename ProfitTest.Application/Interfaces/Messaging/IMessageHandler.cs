namespace ProfitTest.Application.Interfaces.Messaging
{
    public interface IMessageHandler<TMessage>
    {
        Task HandleAsync(TMessage message, CancellationToken cancellationToken);
    }
}
