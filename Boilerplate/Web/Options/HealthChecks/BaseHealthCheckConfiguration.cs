using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Web.Options.HealthChecks
{
    public class BaseHealthCheckConfiguration
    {
        public int Timeout { get; set; }
        public string Name { get; set; }
        public string[] Tags { get; set; }
        public int Status { get; set; }
        public HealthStatus HealthStatus => (HealthStatus) Status;
    }
}
