using System.Collections.Generic;
using System.Linq;
using Kafka.Basic;
using Metrics.MetricData;
using Metrics.Reporters;

namespace Metrics.Kafka
{
    public class TopicReporter : BaseReport
    {
        // We want to keep _client so that it's not disposed and our TCP connection stays alive.
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly IKafkaClient _client;
        private readonly IKafkaTopic _topic;
        private readonly IMapper _mapper;
        private readonly IEncoder _encoder;
        private readonly Compression _codec;

        private string _contextName;
        private List<IKafkaDocument> _data;

        public TopicReporter(string zookeeperConnect, string topicName, IEncoder encoder, Compression codec)
        {
            _client = new KafkaClient(zookeeperConnect);
            _topic = _client.Topic(topicName);
            _encoder = encoder;
            _codec = codec;
            _mapper = new Mapper();
        }

        protected override void StartReport(string contextName)
        {
            _contextName = contextName;
            _data = new List<IKafkaDocument>();
            base.StartReport(contextName);
        }

        protected override void EndReport(string contextName)
        {
            base.EndReport(contextName);
            var messages = _encoder.Encode(contextName, _data, _codec).ToArray();
            _topic.Send(messages);
        }

        protected override void ReportGauge(string name, double value, Unit unit, MetricTags tags)
        {
            _mapper
                .Gauge(name, CurrentContextTimestamp, value, unit, tags)
                .AddTo(_data);
        }

        protected override void ReportCounter(string name, CounterValue value, Unit unit, MetricTags tags)
        {
            _mapper
                .Counter(name, CurrentContextTimestamp, value, unit, tags)
                .AddTo(_data);
        }

        protected override void ReportMeter(string name, MeterValue value, Unit unit, TimeUnit rateUnit, MetricTags tags)
        {
            _mapper
                .Meter(name, CurrentContextTimestamp, value, unit, rateUnit, tags)
                .AddTo(_data);
        }

        protected override void ReportHistogram(string name, HistogramValue value, Unit unit, MetricTags tags)
        {
            _mapper
                .Histogram(name, CurrentContextTimestamp, value, unit, tags)
                .AddTo(_data);
        }

        protected override void ReportTimer(string name, TimerValue value, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
        {
            _mapper
                .Timer(name, CurrentContextTimestamp, value, unit, rateUnit, durationUnit, tags)
                .AddTo(_data);
        }

        protected override void ReportHealth(HealthStatus status)
        {
            _mapper
                .Health(_contextName + "-health", CurrentContextTimestamp, status)
                .AddTo(_data);
        }
    }
}
