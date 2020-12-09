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

        public WelcomeController(ITestClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var result = await _client.GetAsync();
            return Ok(result);
        }
    }
}
