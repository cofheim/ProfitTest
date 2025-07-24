using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProfitTest.Application.Interfaces.Messaging;
using ProfitTest.Infrastructure.Messaging.ConsumerLogic;
using ProfitTest.Infrastructure.Messaging.ProducerLogic;

namespace ProfitTest.Infrastructure.Messaging.Settings
{
    public static class Extensions
    {
        public static void AddProducer<TMessage>(
         this IServiceCollection services,
         IConfigurationSection configSection,
         string topicKey)
        {
            services.Configure<KafkaSettings>(configSection);
            services.AddSingleton<IKafkaProducer<TMessage>>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<KafkaSettings>>();
                return new KafkaProducer<TMessage>(settings, topicKey);
            });
        }

        public static void AddConsumer<TMessage, THandler>(
            this IServiceCollection services,
            IConfigurationSection configSection,
            string topicKey)
            where THandler : class, IMessageHandler<TMessage>
        {
            services.Configure<KafkaSettings>(configSection);
            services.AddScoped<IMessageHandler<TMessage>, THandler>();
            services.AddHostedService(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<KafkaSettings>>();
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                var logger = sp.GetRequiredService<ILogger<KafkaConsumer<TMessage>>>();
                return new KafkaConsumer<TMessage>(settings, scopeFactory, logger, topicKey);
            });
        }
    }
}
