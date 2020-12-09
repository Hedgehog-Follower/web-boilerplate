using System;
using System.Net.Http;
using System.Threading.Tasks;
using Web.Options.Clients;

namespace Web.HttpClients
{
    public interface ITestClient
    {
        Task<string> GetAsync();
    }

    public class TestClient : ITestClient
    {
        private readonly HttpClient _client;
        
        public TestClient(ITestConfiguration configuration, HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri(configuration.BaseAddress);
        }

        public async Task<string> GetAsync()
        {
            return await _client.GetStringAsync("/");
        }
    }
}
