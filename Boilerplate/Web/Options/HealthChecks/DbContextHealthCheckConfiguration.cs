namespace Web.Options.HealthChecks
{
    public class DbContextHealthCheckConfiguration : BaseHealthCheckConfiguration
    {
        public const string SectionPointer = "HealthChecks:Ready:DbContexts:";
    }
}
