using Confluent.Kafka;
using System.Text.Json;

namespace ProfitTest.Infrastructure.Messaging.Settings
{
    public class KafkaJsonDeserializer<TMessage> : IDeserializer<TMessage>
    {
        public TMessage Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            return JsonSerializer.Deserialize<TMessage>(data);
        }
    }
}
