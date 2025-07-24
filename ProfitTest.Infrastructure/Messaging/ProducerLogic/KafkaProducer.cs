using Confluent.Kafka;
using Microsoft.Extensions.Options;
using ProfitTest.Application.Interfaces.Messaging;
using ProfitTest.Infrastructure.Messaging.Settings;
using static Confluent.Kafka.ConfigPropertyNames;

namespace ProfitTest.Infrastructure.Messaging.ProducerLogic
{
    public class KafkaProducer<TMessage> : IKafkaProducer<TMessage>
    {
        private readonly IProducer<string, TMessage> _producer;
        private readonly string _topic;

        public KafkaProducer(IOptions<KafkaSettings> kafkaSettings, string topicKey)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = kafkaSettings.Value.BootstrapServers,
                SecurityProtocol = SecurityProtocol.Plaintext,
                MessageTimeoutMs = 10000
            };

            _producer = new ProducerBuilder<string, TMessage>(config)
                .SetValueSerializer(new KafkaJsonSerializer<TMessage>())
                .Build();

            if (!kafkaSettings.Value.Topics.TryGetValue(topicKey, out var topic))
                throw new ArgumentException($"Topic с ключом {topicKey} не найден в конфигурации");

            _topic = topic;
        }

        public async Task ProduceAsync(TMessage message, CancellationToken cancellationToken)
        {
            try
            {
                await _producer.ProduceAsync(_topic, new Message<string, TMessage>
                {
                    Key = Guid.NewGuid().ToString(),
                    Value = message
                }, cancellationToken);
            }
            catch (ProduceException<string, TMessage> ex)
            {
                throw new Exception($"Ошибка при отправке сообщения в топик {_topic}: {ex.Message}", ex);
            }
        }

        public void Dispose()
        {
            _producer?.Dispose();
        }
    }
}   
