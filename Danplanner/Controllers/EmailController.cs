using Danplanner.Services;
using Microsoft.AspNetCore.Mvc;

namespace Danplanner.Controllers
{
    [ApiController]
    [Route("api/testmail")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _email;

        public EmailController(IEmailService email)
        {
            _email = email;
        }

        [HttpGet]
        public async Task<IActionResult> SendTest()
        {
            await _email.SendAsync("danielgrevsen@live.dk", "Test fra Danplanner", "Hej Daniel, dette er en testmail.");
            return Ok("Mail sendt!");
        }
    }
}
