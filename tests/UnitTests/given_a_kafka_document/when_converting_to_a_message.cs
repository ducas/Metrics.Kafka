using System.Linq;
using FluentAssertions;
using Kafka.Basic;
using Metrics.Kafka;
using NUnit.Framework;

namespace UnitTests.given_a_kafka_document
{
    public class when_converting_to_a_message
    {
        [Test]
        public void then_key_is_set_to_a_combination_of_the_context_type_and_name()
        {
            var document = new KafkaDocument<Meter> { Name = "metric name", Type = "Meter" };
            var encoder = new JsonEncoder();
            var message = encoder.Encode("context", new[] { document });
            message.Single().Key.Should().Be("context:Meter:metric name");
        }

        [Test]
        public void then_properties_are_serialized()
        {
            var document = new KafkaDocument<Counter>
            {
                Name = "mycounter"
            };
            var encoder = new JsonEncoder();
            var message = encoder.Encode("context", new[] { document });
            message.Single().Value.Should().Be("{\"name\":\"mycounter\"}");
        }

        [Test]
        public void then_compression_codec_is_set_to_snappy()
        {
            var document = new KafkaDocument<Counter>();
            var encoder = new JsonEncoder();
            var message = encoder.Encode("context", new[] { document });
            message.Single().Codec.Should().Be(Compression.Snappy);
        }
    }
}
