using System;
using FluentAssertions;
using Metrics;
using Metrics.Kafka;
using Metrics.MetricData;
using NUnit.Framework;
using Counter = Metrics.Kafka.Counter;

namespace UnitTests.given_a_json_encoder
{
    class when_converting_a_counter_to_a_kafka_document
    {
        [Test]
        public void then_a_document_will_be_returned()
        {
            var encoder = new Mapper();
            var timestamp = DateTime.Today;
            var value = new CounterValue(2, new[]
            {
                new CounterValue.SetItem("item1", 1, 10),
                new CounterValue.SetItem("item2", 2, 20),
            });
            var expected = new JsonKafkaDocument<Counter>
            {
                Name = "counter",
                Type = "Counter",
                Timestamp = timestamp,
                Tags = new[] { "tag1", "tag2" },
                Value = new Counter
                {
                    Unit = Unit.Calls,
                    Count = 2L,
                    Items = new[]
                    {
                        new CounterItem
                        {
                            Name = "item1",
                            Count = 1,
                            Percentage = 10D
                        },
                        new CounterItem
                        {
                            Name = "item2",
                            Count = 2,
                            Percentage = 20D
                        }
                    }
                }
            };
            var actual = encoder.Counter("counter", timestamp, value, Unit.Calls, new MetricTags("tag1", "tag2")) as JsonKafkaDocument<Counter>;
            actual.ShouldBeEquivalentTo(expected);
        }
    }
}
