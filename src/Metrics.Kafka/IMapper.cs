using System;
using Metrics.MetricData;

namespace Metrics.Kafka
{
    public interface IMapper
    {
        IKafkaDocument Gauge(string name, DateTime timestamp, double value, Unit unit, MetricTags tags);
        IKafkaDocument Counter(string name, DateTime timestamp, CounterValue value, Unit unit, MetricTags tags);
        IKafkaDocument Meter(string name, DateTime timestamp, MeterValue value, Unit unit, TimeUnit timeUnit, MetricTags tags);
        IKafkaDocument Histogram(string name, DateTime timestamp, HistogramValue value, Unit unit, MetricTags tags);
        IKafkaDocument Timer(string name, DateTime timestamp, TimerValue value, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags);
    }
}