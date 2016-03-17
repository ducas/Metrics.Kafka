using System;
using FluentAssertions;
using Metrics;
using Metrics.Json;
using Metrics.Kafka;
using Metrics.Utils;
using NUnit.Framework;

namespace UnitTests.given_a_json_encoder
{
    namespace when_converting_a_gauge_to_a_kafka_document
    {
        public class and_the_value_is_infinity
        {
            [Test]
            public void then_the_document_will_be_null()
            {
                var encoder = new JsonEncoder();
                var actual = encoder.Gauge("gauge", DateTime.Today, double.PositiveInfinity, Unit.Calls, new MetricTags("tag1", "tag2"));
                actual.Should().BeNull();
            }
        }

        public class and_the_value_of_the_guage_is_nan
        {
            [Test]
            public void then_the_document_will_be_null()
            {
                var encoder = new JsonEncoder();
                var actual = encoder.Gauge("gauge", DateTime.Today, double.NaN, Unit.Calls, new MetricTags("tag1", "tag2"));
                actual.Should().BeNull();
            }
        }

        public class and_the_value_of_the_guage_is_a_valid_number
        {
            [Test]
            public void then_the_a_document_will_be_returned()
            {
                var encoder = new JsonEncoder();
                var timestamp = DateTime.Today;
                var actual = encoder.Gauge("gauge", timestamp, 1, Unit.Calls, new MetricTags("tag1", "tag2")) as JsonKafkaDocument;
                var expected = new JsonKafkaDocument
                {
                    Properties = new JsonObject(new[]
                    {
                        new JsonProperty("Timestamp", Clock.FormatTimestamp(timestamp)),
                        new JsonProperty("Type", "Gauge"),
                        new JsonProperty("Name", "gauge"),
                        new JsonProperty("Unit", "Calls"),
                        new JsonProperty("Tags", new[] { "tag1", "tag2" }),
                        new JsonProperty("Value", 1D)
                    })
                };
                actual.Properties.AsJson().Should().Be(expected.Properties.AsJson());
            }
        }
    }
}
