using System;
using System.Collections.Generic;
using System.Linq;
using Metrics.Json;
using Metrics.MetricData;
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

        public IKafkaDocument Counter(string name, DateTime timestamp, CounterValue value, Unit unit, MetricTags tags)
        {
            var itemProperties = value.Items.SelectMany(i => new[]
            {
                new JsonProperty(i.Item + " - Count", i.Count),
                new JsonProperty(i.Item + " - Percent", i.Percent),
            });

            return Pack("Counter", name, timestamp, unit, tags, new[]
            {
                new JsonProperty("Count", value.Count),
            }.Concat(itemProperties));
        }

        public IKafkaDocument Meter(string name, DateTime timestamp, MeterValue value, Unit unit, TimeUnit timeUnit, MetricTags tags)
        {
            return new JsonKafkaDocument<Meter>
            {
                Type = "Meter",
                Name = name,
                Timestamp = timestamp,
                Tags = tags.Tags,
                Value = new Meter
                {
                    Unit = unit,
                    TimeUnit = timeUnit,
                    Current = new MeterItem
                    {
                        Count = value.Count,
                        MeanRate = value.MeanRate,
                        OneMinuteRate = value.OneMinuteRate,
                        FiveMinuteRate = value.FiveMinuteRate,
                        FifteenMinuteRate = value.FifteenMinuteRate
                    },
                    Items = value.Items
                        .Select(i => new MeterItem
                        {
                            Count = i.Value.Count,
                            Percent = i.Percent,
                            MeanRate = i.Value.MeanRate,
                            OneMinuteRate = i.Value.OneMinuteRate,
                            FiveMinuteRate = i.Value.FiveMinuteRate,
                            FifteenMinuteRate = i.Value.FifteenMinuteRate
                        })
                        .ToArray()
                }
            };
        }
    }
}
