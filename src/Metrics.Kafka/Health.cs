using System.Collections.Generic;

namespace Metrics.Kafka
{
    public class Health
    {
        public bool IsHealthy { get; set; }
        public bool HasRegisteredChecks { get; set; }
        public IEnumerable<HealthCheck> Results { get; set; }
    }

    public class HealthCheck
    {
        public string Name { get; set; }
        public bool IsHealthy { get; set; }
        public string Message { get; set; }
    }
}
