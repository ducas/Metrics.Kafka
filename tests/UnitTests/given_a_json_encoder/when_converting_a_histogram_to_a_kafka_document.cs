using System;
using FluentAssertions;
using Metrics;
using Metrics.Kafka;
using Metrics.MetricData;
using NUnit.Framework;
using Histogram = Metrics.Kafka.Histogram;

namespace UnitTests.given_a_json_encoder
{
    public class when_converting_a_histogram_to_a_kafka_document
    {
        [Test]
        public void then_a_document_will_be_returned()
        {
            var encoder = new Mapper();
            var timestamp = DateTime.Today;
            var value = new HistogramValue(1, 2, "3", 4, "5", 6, 7, "8", 9, 10, 11, 12, 13, 14, 15, 16);
            var expected = new JsonKafkaDocument<Histogram>
            {
                Name = "histogram",
                Timestamp = timestamp,
                Type = "Histogram",
                Tags = new[] { "tag1", "tag2" },
                Value = new Histogram
                {
                    Unit = Unit.Calls,
                    Count = 1,
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

            var actual = encoder.Histogram("histogram", timestamp, value, Unit.Calls, new MetricTags("tag1", "tag2")) as JsonKafkaDocument<Histogram>;
            
            actual.ShouldBeEquivalentTo(expected);
        }
    }
}
