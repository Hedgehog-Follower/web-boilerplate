using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Registry;
using Web.Options.Clients;
using Web.Policies;

namespace Web.HttpClients
{
    public interface ITestClient
    {
        Task<string> GetAsync();
    }

    public class TestClient : ITestClient
    {
        private readonly HttpClient _client;
        private readonly IReadOnlyPolicyRegistry<string> _policyRegistry;
        private readonly ILogger<TestClient> _logger;

        public TestClient(ITestConfiguration configuration, HttpClient client, IReadOnlyPolicyRegistry<string> policyRegistry, ILogger<TestClient> logger)
        {
            _client = client;
            _policyRegistry = policyRegistry;
            _logger = logger;
            _client.BaseAddress = new Uri(configuration.BaseAddress);
        }

        public async Task<string> GetAsync()
        {
            var retryPolicy = _policyRegistry.Get<IAsyncPolicy<HttpResponseMessage>>(PolicyNames.RetryWithLogging)
                              ?? Policy.NoOpAsync<HttpResponseMessage>();

            var context = new Context(
                $"{ContextNames.Logger}-{Guid.NewGuid()}",
                new Dictionary<string, object>
                    {
                        { ContextNames.Logger, _logger }
                    });

            var response = await retryPolicy.ExecuteAsync(ctx => _client.GetAsync("/ds"), context);

            return response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : "Error";
        }
    }
}
