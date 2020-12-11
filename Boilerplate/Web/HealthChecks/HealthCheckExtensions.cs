using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Web.Options.HealthChecks;

namespace Web.HealthChecks
{
    public static class HealthCheckExtensions
    {
        public static IHealthChecksBuilder AddUriHealthCheck(
            this IHealthChecksBuilder builder, IConfigurationSection configurationSection)
        {
            if (configurationSection == null)
            {
                throw new ArgumentNullException(nameof(configurationSection));
            }

            var configuration = new UriHealthCheckConfiguration();
            configurationSection.Bind(configuration);

            return 
                builder
                    .AddUrlGroup(
                        new Uri(configuration.BaseAddress),
                        configuration.Name,
                        (HealthStatus)configuration.Status,
                        configuration.Tags, 
                        TimeSpan.FromSeconds(configuration.Timeout));
        }
    }
}
