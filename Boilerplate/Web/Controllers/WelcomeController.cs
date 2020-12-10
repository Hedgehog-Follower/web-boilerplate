using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.HttpClients;

namespace Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WelcomeController : ControllerBase
    {
        private readonly ITestClient _client;
        private readonly IWelcomeClient _welcomeClient;

        public WelcomeController(ITestClient client, IWelcomeClient welcomeClient)
        {
            _client = client;
            _welcomeClient = welcomeClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            
            var result1 = await _welcomeClient.GetAsync();
            var result = await _client.GetAsync();
            return Ok(result);
        }
    }
}
