using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProfitTest.Application.Interfaces.Messaging;
using ProfitTest.Infrastructure.Messaging.Settings;

namespace ProfitTest.Infrastructure.Messaging.ConsumerLogic
{
    public class KafkaConsumer<TMessage> : BackgroundService
    {
        private readonly IConsumer<string, TMessage> _consumer;
        private readonly IMessageHandler<TMessage> _messageHandler;
        private readonly ILogger<KafkaConsumer<TMessage>> _logger;
        private readonly string _topic;

        public KafkaConsumer(
        IOptions<KafkaSettings> kafkaSettings,
        IMessageHandler<TMessage> messageHandler,
        ILogger<KafkaConsumer<TMessage>> logger,
        string topicKey)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = kafkaSettings.Value.BootstrapServers,
                GroupId = kafkaSettings.Value.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            if (!kafkaSettings.Value.Topics.TryGetValue(topicKey, out var topic))
                throw new ArgumentException($"Topic с ключом {topicKey} не найден в конфигурации");

            _topic = topic;
            _messageHandler = messageHandler;
            _logger = logger;
            _consumer = new ConsumerBuilder<string, TMessage>(config)
                .SetValueDeserializer(new KafkaJsonDeserializer<TMessage>())
                .Build();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() => StartConsumerLoop(stoppingToken), stoppingToken);
        }

        private async Task StartConsumerLoop(CancellationToken cancellationToken)
        {
            _consumer.Subscribe(_topic);

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var result = _consumer.Consume(cancellationToken);
                        await _messageHandler.HandleAsync(result.Message.Value, cancellationToken);
                    }
                    catch (ConsumeException ex)
                    {
                        _logger.LogError(ex, "Ошибка при получении сообщения из топика {Topic}", _topic);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Ошибка при обработке сообщения из топика {Topic}", _topic);
                    }
                }
            }
            finally
            {
                _consumer.Close();
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _consumer?.Dispose();
            return base.StopAsync(cancellationToken);
        }
    }
}   
