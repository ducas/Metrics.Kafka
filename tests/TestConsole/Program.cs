using System;
using System.Threading;
using CommandLine;
using Metrics;
using Metrics.Kafka;

namespace TestConsole
{
    class Program
    {
        static int Main(string[] args)
        {
            return Parser.Default
                .ParseArguments<Options>(args)
                .MapResult(Run, e => 1);
        }

        private static readonly Random Rand = new Random();
        private static int Run(Options options)
        {
            Thread.Sleep(5000);
            Metric.Gauge("gauge", () => Rand.NextDouble() * 100, Unit.Bytes);
            var counter = Metric.Counter("counter", Unit.Items);
            var meter = Metric.Meter("meter", Unit.Calls);
            var histogram = Metric.Histogram("histogram", Unit.Commands);
            var timer = Metric.Timer("timer", Unit.Events);
            HealthChecks.RegisterHealthCheck("health", () =>
            {
                var value = Rand.Next(100);
                if (value < 30) return HealthCheckResult.Unhealthy("Oh-oh...");
                if (value < 60) return HealthCheckResult.Unhealthy("I'm going down!");
                if (value < 90) return HealthCheckResult.Healthy("Pretty good...");
                return HealthCheckResult.Healthy("Exceptional!");
            });

            Metric.Config.WithReporting(r => r.WithKafka(
                options.ZkConnect,
                options.Topic,
                TimeSpan.FromSeconds(options.Report)));

            var start = DateTime.Now;
            var runFor = TimeSpan.FromSeconds(options.Duration);
            while (DateTime.Now - start < runFor)
            {
                counter.Increment();
                meter.Mark();
                histogram.Update(Rand.Next(1000));
                timer.Record(Rand.Next(1000), TimeUnit.Milliseconds);
                Thread.Sleep(options.Sleep);
            }

            return 0;
        }
    }
}
