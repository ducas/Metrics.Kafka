using System;
using Kafka.Basic;
using Metrics.Reports;

namespace Metrics.Kafka
{
    public static class MetricsReportsExtensions
    {
        /// <summary>
        /// Scheduled a Kafka Topic Reporter to be executed at a fixed interval. 
        /// </summary>
        /// <param name="reports">The metrics reports this reporter will be added to.</param>
        /// <param name="zookeeperConnect">The connection string for zookeeper.</param>
        /// <param name="topicName">The name of the topic to publish to.</param>
        /// <param name="interval">The interval at which to publish.</param>
        /// <param name="encoder">The encoder to use when serializing reports. Default - Metrics.Kafka.JsonEncoder (uses Newtonsoft.Json).</param>
        /// <param name="codec">The compression codec to use when publishing messages. Default - Snappy.</param>
        /// <returns></returns>
        public static MetricsReports WithKafka(this MetricsReports reports,
            string zookeeperConnect,
            string topicName,
            TimeSpan interval,
            IEncoder encoder = null,
            Compression codec = Compression.Snappy
            )
        {
            return reports.WithReport(
                new TopicReporter(zookeeperConnect, topicName, encoder ?? new JsonEncoder(), codec),
                interval);
        }
    }
}
