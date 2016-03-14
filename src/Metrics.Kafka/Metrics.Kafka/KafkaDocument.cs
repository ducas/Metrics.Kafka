using Kafka.Basic;
using Metrics.Json;

namespace Metrics.Kafka
{
    public class KafkaDocument
    {
        public JsonObject Properties { get; set; }

        public Message ToMessage(string contextName)
        {
            return new Message
            {
                Key = contextName,
                Value = Properties.AsJson(false)
            };
        }
    }
}