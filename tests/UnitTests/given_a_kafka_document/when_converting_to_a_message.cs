using FluentAssertions;
using Kafka.Basic;
using Metrics.Json;
using Metrics.Kafka;
using NUnit.Framework;

namespace UnitTests.given_a_kafka_document
{
    public class when_converting_to_a_message
    {
        [Test]
        public void then_key_is_set_to_the_context()
        {
            var document = new JsonKafkaDocument();
            var message = document.ToMessage("context");
            message.Key.Should().Be("context");
        }

        [Test]
        public void then_properties_are_serialized()
        {
            var document = new JsonKafkaDocument
            {
                Properties = new JsonObject(new[]
                {
                    new JsonProperty("property","value")
                })
            };
            var message = document.ToMessage("context");
            message.Value.Should().Be("{\"property\":\"value\"}");
        }

        [Test]
        public void then_compression_codec_is_set_to_snappy()
        {
            var document = new JsonKafkaDocument();
            var message = document.ToMessage("context");
            message.Codec.Should().Be(Compression.Snappy);
        }
    }
}
