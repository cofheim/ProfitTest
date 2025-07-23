using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProfitTest.Application.Interfaces.Messaging;
using ProfitTest.Infrastructure.Messaging.ConsumerLogic;
using ProfitTest.Infrastructure.Messaging.ProducerLogic;

namespace ProfitTest.Infrastructure.Messaging.Settings
{
    public static class Extensions
    {
        public static void AddProducer<TMessage>(this IServiceCollection serviceCollection,
            IConfigurationSection configurationSection)
        {
            serviceCollection.Configure<KafkaSettings>(configurationSection);
            serviceCollection.AddSingleton<IKafkaProducer<TMessage>, KafkaProducer<TMessage>>();
        }

        public static IServiceCollection AddConsumer<TMessage, THandler>(this IServiceCollection serviceCollection,
            IConfigurationSection configurationSection) where THandler : class, IMessageHandler<TMessage>
        {
            serviceCollection.Configure<KafkaSettings>(configurationSection);
            serviceCollection.AddHostedService<KafkaConsumer<TMessage>>();
            serviceCollection.AddSingleton<IMessageHandler<TMessage>, THandler>();

            return serviceCollection;
        }
    }
}
