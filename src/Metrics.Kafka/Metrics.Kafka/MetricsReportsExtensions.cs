using System;
using Metrics.Reports;

namespace Metrics.Kafka
{
    public static class MetricsReportsExtensions
    {
        public static MetricsReports WithKafka(this MetricsReports reports, string zookeeperConnect, string topicName, TimeSpan interval)
        {
            return reports.WithReport(new TopicReporter(zookeeperConnect, topicName), interval);
        }
    }
}
