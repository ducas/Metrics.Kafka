using System;
using Kafka.Basic;
using Metrics.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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

    public class JsonKafkaDocument<T> : IKafkaDocument<T>
    {
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.None,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public string Type { get; set; }
        public string Name { get; set; }
        public DateTime Timestamp { get; set; }
        public string[] Tags { get; set; }
        public T Value { get; set; }

        public Message ToMessage(string contextName)
        {
            return new Message
            {
                Key = contextName,
                Value = JsonConvert.SerializeObject(this, SerializerSettings),
                Codec = Compression.Snappy
            };
        }
    }
}