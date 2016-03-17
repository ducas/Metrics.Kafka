using System;

namespace Metrics.Kafka
{
    public interface IEncoder
    {
        IKafkaDocument Gauge(string name, DateTime timestamp, double value, Unit unit, MetricTags tags);
    }
}