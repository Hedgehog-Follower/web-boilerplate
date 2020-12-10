using System;
using System.Net.Http;
using System.Threading.Tasks;
using Web.Options.Clients;

namespace Web.HttpClients
{
    public interface IWelcomeClient
    {
        Task<string> GetAsync();
    }

    public class WelcomeClient : IWelcomeClient
    {
        private readonly HttpClient _client;

        public WelcomeClient(ITestConfiguration configuration, HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri(configuration.BaseAddress);
        }

        public async Task<string> GetAsync()
        {
            var result = await _client.GetAsync("/re");

            return result.IsSuccessStatusCode ? result.Content.ToString() : "Error";
        }
    }
}
