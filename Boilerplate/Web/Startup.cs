using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using Hellang.Middleware.ProblemDetails;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Polly;
using Web.Handlers;
using Web.HttpClients;
using Web.Options.Clients;
using Web.Policies;

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
                    opts.IncludeExceptionDetails = (ctx, ex) => Environment.IsDevelopment();
                })
                .AddControllers();



            services
                .Configure<TestConfiguration>(Configuration.GetSection(TestConfiguration.ConfigurationName));

            services.TryAddSingleton<ITestConfiguration>(sp =>
                sp.GetRequiredService<IOptions<TestConfiguration>>().Value);

            services
                .AddPollyPolicies()
                .AddHttpClient<ITestClient, TestClient>()
                .AddPolicyHandlerFromRegistry(PolicyNames.Timeout)
                .RegisterAndAddHttpMessageHandler<ValidateHeaderHandler>();

            services
                .AddHttpClient<IWelcomeClient, WelcomeClient>()
                .AddPolicyHandlerFromRegistry(PolicyNames.RetryWithLogging)
                .AddPolicyHandlerFromRegistry(PolicyNames.Timeout)
                .RegisterAndAddHttpMessageHandler<ValidateHeaderHandler>();

            services.AddSingleton<IHttpClientForSingletonConsumers, HttpClientForSingletonConsumers>();
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
