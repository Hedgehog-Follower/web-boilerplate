using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
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

            var response = await _client.GetAsync("/re", HttpCompletionOption.ResponseHeadersRead);

            var stream = await response.Content.ReadAsStreamAsync();
            var configuration = stream.ReadAndDeserializeFromJson<TestConfiguration>();

            return string.Empty;
        }
    }
}
