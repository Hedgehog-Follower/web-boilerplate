using System;
using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Web.Hosts
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var environment = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();
            if (environment.IsDevelopment())
            {
                using var applicationContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

                try
                {
                    applicationContext.Database.Migrate();
                }
                catch (Exception ex)
                {
                    // Log here when migration fail
                    throw;
                }
            }

            return host;
        }
    }
}
