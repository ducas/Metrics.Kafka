using System;
using FluentAssertions;
using Metrics;
using Metrics.Kafka;
using Metrics.MetricData;
using NUnit.Framework;
using Timer = Metrics.Kafka.Timer;

namespace UnitTests.given_a_json_encoder
{
    public class when_converting_a_health_report_to_a_kafka_document
    {
        [Test]
        public void then_a_document_will_be_returned()
        {
            var encoder = new Mapper();
            var timestamp = DateTime.Today;
            
            var value = new HealthStatus(new[]
            {
                new Metrics.Core.HealthCheck("check1", () => "no message").Execute(),
                new Metrics.Core.HealthCheck("check2", () => HealthCheckResult.Unhealthy("some message")).Execute()
            });
            var expected = new JsonKafkaDocument<Health>
            {
                Name = "health",
                Timestamp = timestamp,
                Type = "Health",
                Value = new Health
                {
                    IsHealthy = false,
                    HasRegisteredChecks = true,
                    Results = new[]
                    {
                        new HealthCheck { Name = "check1", IsHealthy = true, Message = "no message" },
                        new HealthCheck { Name = "check2", IsHealthy = false, Message = "some message" },
                    }
                }
            };

            var actual = encoder.Health("health", timestamp, value) as JsonKafkaDocument<Health>;

            actual.ShouldBeEquivalentTo(expected);
        }
    }
}