using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<KafkaConsumer<TMessage>> _logger;
        private readonly string _topic;
        private readonly string _bootstrapServers;

        public KafkaConsumer(
            IOptions<KafkaSettings> kafkaSettings,
            IServiceScopeFactory serviceScopeFactory,
            ILogger<KafkaConsumer<TMessage>> logger,
            string topicKey)
        {
            _bootstrapServers = kafkaSettings.Value.BootstrapServers;
            var config = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                GroupId = kafkaSettings.Value.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                SecurityProtocol = SecurityProtocol.Plaintext,
                AllowAutoCreateTopics = true
            };

            if (!kafkaSettings.Value.Topics.TryGetValue(topicKey, out var topic))
                throw new ArgumentException($"Topic с ключом {topicKey} не найден в конфигурации");

            _topic = topic;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _consumer = new ConsumerBuilder<string, TMessage>(config)
                .SetValueDeserializer(new KafkaJsonDeserializer<TMessage>())
                .Build();

            EnsureTopicExists().GetAwaiter().GetResult();
        }

        private async Task EnsureTopicExists()
        {
            try
            {
                using (var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = _bootstrapServers }).Build())
                {
                    var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(10));
                    if (!metadata.Topics.Any(t => t.Topic == _topic))
                    {
                        await adminClient.CreateTopicsAsync(new TopicSpecification[]
                        {
                            new TopicSpecification
                            {
                                Name = _topic,
                                ReplicationFactor = 1,
                                NumPartitions = 1
                            }
                        });
                        _logger.LogInformation("Топик {Topic} создан", _topic);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании топика {Topic}", _topic);
            }
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
                        
                        using (var scope = _serviceScopeFactory.CreateScope())
                        {
                            var handler = scope.ServiceProvider.GetRequiredService<IMessageHandler<TMessage>>();
                            await handler.HandleAsync(result.Message.Value, cancellationToken);
                        }
                    }
                    catch (ConsumeException ex)
                    {
                        _logger.LogError(ex, "Ошибка при получении сообщения из топика {Topic}", _topic);
                        await Task.Delay(1000, cancellationToken); // Добавляем задержку при ошибке
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Ошибка при обработке сообщения из топика {Topic}", _topic);
                        await Task.Delay(1000, cancellationToken); // Добавляем задержку при ошибке
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
