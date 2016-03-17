using Kafka.Basic;
using Metrics.Json;

namespace Metrics.Kafka
{
    public class JsonKafkaDocument : IKafkaDocument
    {
        public JsonObject Properties { get; set; }

        public Message ToMessage(string contextName)
        {
            return new Message
            {
                Key = contextName,
                Value = Properties?.AsJson(false),
                Codec = Compression.Snappy
            };
        }
    }
}