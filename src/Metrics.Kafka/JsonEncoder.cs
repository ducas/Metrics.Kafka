using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kafka.Basic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Metrics.Kafka
{
    public interface IEncoder
    {
        IEnumerable<Message> Encode(string contextName, IEnumerable<IKafkaDocument> documents, Compression codec = Compression.Default);
    }

    public class JsonEncoder : IEncoder
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.None,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public IEnumerable<Message> Encode(string contextName, IEnumerable<IKafkaDocument> documents, Compression codec = Compression.Snappy)
        {
            return documents.Select(d => new Message
            {
                Key = contextName,
                Value = JsonConvert.SerializeObject(d, Settings),
                Codec = codec
            });
        }
    }
}
