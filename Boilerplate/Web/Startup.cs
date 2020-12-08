using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hellang.Middleware.ProblemDetails;
using Microsoft.Extensions.Configuration;

namespace Web
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddProblemDetails(opts =>
                {
                    opts.IncludeExceptionDetails = (ctx, ex) => !Environment.IsDevelopment();
                })
                .AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseProblemDetails();

            // Endpoint routing separates the process of selecting which "endpoint" will execute from the actual running of that endpoint.
            // An endpoint consists of a path pattern, and something to execute when called.

            // The UseRouting() extension method is what looks at the incoming request and decides which endpoint should execute.
            // Any middleware that appears after the UseRouting() call will know which endpoint will run eventually.
            app.UseRouting();

            // The UseEndpoints() call is responsible for configuring the endpoints, but also for executing them.
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
