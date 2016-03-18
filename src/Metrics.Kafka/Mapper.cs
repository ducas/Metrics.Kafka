using System;
using System.Collections.Generic;
using System.Linq;
using Metrics.Json;
using Metrics.MetricData;
using Metrics.Utils;

namespace Metrics.Kafka
{
    public class Mapper : IMapper
    {
        public IKafkaDocument Gauge(string name, DateTime timestamp, double value, Unit unit, MetricTags tags)
        {
            if (!double.IsNaN(value) && !double.IsInfinity(value))
            {
                return new JsonKafkaDocument<Gauge>
                {
                    Name = name,
                    Type = "Gauge",
                    Timestamp = timestamp,
                    Tags = tags.Tags,
                    Value = new Gauge
                    {
                        Unit = unit,
                        Value = value
                    }
                };
            }
            return null;
        }

        public IKafkaDocument Counter(string name, DateTime timestamp, CounterValue value, Unit unit, MetricTags tags)
        {
            return new JsonKafkaDocument<Counter>
            {
                Name = name,
                Type = "Counter",
                Timestamp = timestamp,
                Tags = tags.Tags,
                Value = new Counter
                {
                    Unit = unit,
                    Count = value.Count,
                    Items = value.Items.Select(i => new CounterItem
                    {
                        Name = i.Item,
                        Count = i.Count,
                        Percentage = i.Percent
                    }).ToArray()
                }
            };
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

        public IKafkaDocument Histogram(string name, DateTime timestamp, HistogramValue value, Unit unit, MetricTags tags)
        {
            return new JsonKafkaDocument<Histogram>
            {
                Name = name,
                Timestamp = timestamp,
                Type = "Histogram",
                Tags = tags.Tags,
                Value = new Histogram
                {
                    Unit = unit,
                    Count = value.Count,
                    Last = value.LastValue,
                    LastUserValue = value.LastUserValue,
                    Max = value.Max,
                    MaxUserValue = value.MaxUserValue,
                    Mean = value.Mean,
                    Min = value.Min,
                    MinUserValue = value.MinUserValue,
                    StdDev = value.StdDev,
                    Median = value.Median,
                    Percentile75 = value.Percentile75,
                    Percentile95 = value.Percentile95,
                    Percentile98 = value.Percentile98,
                    Percentile99 = value.Percentile99,
                    Percentile999 = value.Percentile999,
                    SampleSize = value.SampleSize
                }
            };
        }

        public IKafkaDocument Timer(string name, DateTime timestamp, TimerValue value, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
        {
            return new JsonKafkaDocument<Timer>
            {
                Name = name,
                Timestamp = timestamp,
                Type = "Timer",
                Tags = tags.Tags,
                Value = new Timer
                {
                    Unit = unit,
                    RateUnit = rateUnit,
                    DurationUnit = durationUnit,
                    ActiveSessions = value.ActiveSessions,
                    Count = value.Rate.Count,
                    Last = value.Histogram.LastValue,
                    LastUserValue = value.Histogram.LastUserValue,
                    Max = value.Histogram.Max,
                    MaxUserValue = value.Histogram.MaxUserValue,
                    Mean = value.Histogram.Mean,
                    Min = value.Histogram.Min,
                    MinUserValue = value.Histogram.MinUserValue,
                    StdDev = value.Histogram.StdDev,
                    Median = value.Histogram.Median,
                    Percentile75 = value.Histogram.Percentile75,
                    Percentile95 = value.Histogram.Percentile95,
                    Percentile98 = value.Histogram.Percentile98,
                    Percentile99 = value.Histogram.Percentile99,
                    Percentile999 = value.Histogram.Percentile999,
                    SampleSize = value.Histogram.SampleSize,
                    MeanRate = value.Rate.MeanRate,
                    OneMinRate = value.Rate.OneMinuteRate,
                    FiveMinRate = value.Rate.FiveMinuteRate,
                    FifteenMinRate = value.Rate.FifteenMinuteRate
                }
            };
        }
    }
}
