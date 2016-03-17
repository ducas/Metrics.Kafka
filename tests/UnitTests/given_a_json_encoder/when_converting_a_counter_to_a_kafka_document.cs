using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Metrics;
using Metrics.Json;
using Metrics.Kafka;
using Metrics.MetricData;
using Metrics.Utils;
using NUnit.Framework;

namespace UnitTests.given_a_json_encoder
{
    class when_converting_a_counter_to_a_kafka_document
    {
        [Test]
        public void then_a_document_will_be_returned()
        {
            var encoder = new JsonEncoder();
            var timestamp = DateTime.Today;
            var value = new CounterValue(2, new[]
            {
                new CounterValue.SetItem("item1", 1, 10),
                new CounterValue.SetItem("item2", 2, 20),
            });
            var expected = new JsonObject(new []
            {
                new JsonProperty("Timestamp", Clock.FormatTimestamp(timestamp)),
                new JsonProperty("Type", "Counter"),
                new JsonProperty("Name", "counter"),
                new JsonProperty("Unit", "Calls"),
                new JsonProperty("Tags", new[] { "tag1", "tag2" }),
                new JsonProperty("Count", 2),
                new JsonProperty("item1 - Count", 1),
                new JsonProperty("item1 - Percent", 10D),
                new JsonProperty("item2 - Count", 2),
                new JsonProperty("item2 - Percent", 20D),
            });
            var actual = encoder.Counter("counter", timestamp, value, Unit.Calls, new MetricTags("tag1", "tag2")) as JsonKafkaDocument;
            actual.Properties.AsJson().Should().Be(expected.AsJson());
        }
    }
}
