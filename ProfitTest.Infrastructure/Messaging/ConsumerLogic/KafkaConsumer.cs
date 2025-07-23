using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ProfitTest.Application.Interfaces.Messaging;
using ProfitTest.Infrastructure.Messaging.Settings;

namespace ProfitTest.Infrastructure.Messaging.ConsumerLogic
{
    public class KafkaConsumer<TMessage> : BackgroundService
    {
        private readonly string _topic;
        private readonly IConsumer<string, TMessage> _consumer;
        private readonly IMessageHandler<TMessage> _messageHandler;

        public KafkaConsumer(IOptions<KafkaSettings> kafkaSettings, IMessageHandler<TMessage> messageHandler)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = kafkaSettings.Value.BootstrapServers,
                GroupId = kafkaSettings.Value.GroupId
            };
            _topic = kafkaSettings.Value.Topic;
            _messageHandler = messageHandler;

            _consumer = new ConsumerBuilder<string, TMessage>(config).SetValueDeserializer(new KafkaJsonDeserializer<TMessage>()).Build();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() => ConsumeAsync(stoppingToken), stoppingToken);
        }

        private async Task? ConsumeAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe(_topic);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var result = _consumer.Consume(stoppingToken);
                    await _messageHandler.HandleAsync(result.Message.Value, stoppingToken);

                }
            }
            catch (Exception ex)
            {
                //
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _consumer.Close();
            return base.StopAsync(cancellationToken);
        }
    }
}   
