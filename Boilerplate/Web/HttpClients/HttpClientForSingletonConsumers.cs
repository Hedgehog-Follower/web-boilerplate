using Microsoft.Extensions.DependencyInjection;

namespace Web.HttpClients
{
    public class HttpClientForSingletonConsumers : IHttpClientForSingletonConsumers
    {
        private readonly ServiceProvider _serviceProvider;

        public HttpClientForSingletonConsumers(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ITestClient GetTestClient()
        {
            return _serviceProvider.GetRequiredService<ITestClient>();
        }
    }

    public interface IHttpClientForSingletonConsumers
    {
        ITestClient GetTestClient();
    }
}
