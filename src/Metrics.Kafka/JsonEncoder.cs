using System;
using System.Collections.Generic;
using System.Linq;
using Metrics.Json;
using Metrics.Utils;

namespace Metrics.Kafka
{
    public class JsonEncoder : IEncoder
    {
        public IKafkaDocument Gauge(string name, DateTime timestamp, double value, Unit unit, MetricTags tags)
        {
            if (!double.IsNaN(value) && !double.IsInfinity(value))
            {
               return  Pack("Gauge", name, timestamp, unit, tags, new[] {
                    new JsonProperty("Value", value),
                });
            }
            return null;
        }

        private IKafkaDocument Pack(string type, string name, DateTime timestamp, Unit unit, MetricTags tags, IEnumerable<JsonProperty> properties)
        {
            return new JsonKafkaDocument
            {
                Properties = new JsonObject(new[]
                {
                    new JsonProperty("Timestamp", Clock.FormatTimestamp(timestamp)),
                    new JsonProperty("Type", type),
                    new JsonProperty("Name", name),
                    new JsonProperty("Unit", unit.ToString()),
                    new JsonProperty("Tags", tags.Tags)
                }.Concat(properties))
            };
        }
    }
}
