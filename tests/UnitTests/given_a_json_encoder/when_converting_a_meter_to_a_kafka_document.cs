using System;
using FluentAssertions;
using Metrics;
using Metrics.Kafka;
using Metrics.MetricData;
using NUnit.Framework;
using Meter = Metrics.Kafka.Meter;

namespace UnitTests.given_a_json_encoder
{
    public class when_converting_a_meter_to_a_kafka_document
    {
        [Test]
        public void then_a_document_will_be_returned()
        {
            var encoder = new Mapper();
            var timestamp = DateTime.Today;
            var value = new MeterValue(
                1, 2, 3, 4, 5, TimeUnit.Seconds, new[]
            {
                new MeterValue.SetItem("item1", 1, new MeterValue(10, 20, 30, 40, 50, TimeUnit.Seconds, new MeterValue.SetItem[0])),
                new MeterValue.SetItem("item2", 2, new MeterValue(100, 200, 300, 400, 500, TimeUnit.Seconds, new MeterValue.SetItem[0])),
            });
            var expected = new KafkaDocument<Meter>
            {
                Name = "meter",
                Timestamp = timestamp,
                Type = "Meter",
                Tags = new[] { "tag1", "tag2" },
                Value = new Meter
                {
                    Unit = Unit.Calls,
                    TimeUnit = TimeUnit.Seconds,
                    Current = new MeterItem
                    {
                        Count = 1,
                        MeanRate = 2,
                        OneMinuteRate = 3,
                        FiveMinuteRate = 4,
                        FifteenMinuteRate = 5
                    },
                    Items = new[]
                    {
                        new MeterItem
                        {
                            Count = 10,
                            Percent = 1,
                            MeanRate = 20,
                            OneMinuteRate = 30,
                            FiveMinuteRate = 40,
                            FifteenMinuteRate = 50
                        },
                        new MeterItem
                        {
                            Count = 100,
                            Percent = 2,
                            MeanRate = 200,
                            OneMinuteRate = 300,
                            FiveMinuteRate = 400,
                            FifteenMinuteRate = 500
                        }
                    }
                }
            };

            var actual = encoder.Meter("meter", timestamp, value, Unit.Calls, TimeUnit.Seconds, new MetricTags("tag1", "tag2")) as KafkaDocument<Meter>;
            
            actual.ShouldBeEquivalentTo(expected);
        }
    }
}
