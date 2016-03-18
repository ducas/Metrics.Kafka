using System;
using FluentAssertions;
using Metrics;
using Metrics.Kafka;
using Metrics.MetricData;
using NUnit.Framework;
using Timer = Metrics.Kafka.Timer;

namespace UnitTests.given_a_json_encoder
{
    public class when_converting_a_timer_to_a_kafka_document
    {
        [Test]
        public void then_a_document_will_be_returned()
        {
            var encoder = new Mapper();
            var timestamp = DateTime.Today;
            var value = new TimerValue(
                new MeterValue(1, 2, 3, 4, 5, TimeUnit.Seconds, new MeterValue.SetItem[0]),
                new HistogramValue(1, 2, "3", 4, "5", 6, 7, "8", 9, 10, 11, 12, 13, 14, 15, 16),
                10,
                TimeUnit.Seconds
                );
            var expected = new JsonKafkaDocument<Timer>
            {
                Name = "timer",
                Timestamp = timestamp,
                Type = "Timer",
                Tags = new[] { "tag1", "tag2" },
                Value = new Timer
                {
                    Unit = Unit.Calls,
                    RateUnit = TimeUnit.Seconds,
                    DurationUnit = TimeUnit.Seconds,
                    Count = 1,
                    ActiveSessions = 10,
                    MeanRate = 2D,
                    OneMinRate = 3D,
                    FiveMinRate = 4D,
                    FifteenMinRate = 5D,
                    Last = 2D,
                    LastUserValue = "3",
                    Max = 4D,
                    MaxUserValue = "5",
                    Mean = 6D,
                    Min = 7D,
                    MinUserValue = "8",
                    StdDev = 9D,
                    Median = 10D,
                    Percentile75 = 11D,
                    Percentile95 = 12D,
                    Percentile98 = 13D,
                    Percentile99 = 14D,
                    Percentile999 = 15D,
                    SampleSize = 16
                }
            };

            var actual = encoder.Timer("timer", timestamp, value, Unit.Calls, TimeUnit.Seconds, TimeUnit.Seconds, new MetricTags("tag1", "tag2")) as JsonKafkaDocument<Timer>;
            
            actual.ShouldBeEquivalentTo(expected);
        }
    }
}