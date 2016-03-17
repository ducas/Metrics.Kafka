using System;
using Metrics.MetricData;

namespace Metrics.Kafka
{
    public interface IEncoder
    {
        IKafkaDocument Gauge(string name, DateTime timestamp, double value, Unit unit, MetricTags tags);
        IKafkaDocument Counter(string name, DateTime timestamp, CounterValue value, Unit unit, MetricTags tags);
    }
}