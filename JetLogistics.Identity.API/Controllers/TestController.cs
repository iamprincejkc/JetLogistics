using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JetLogistics.Identity.API.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        [Authorize]
        [HttpGet("secure")]
        public IActionResult SecureEndpoint()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            return Ok(new { message = $"Hello {username}, you accessed a protected endpoint!" });
        }

        [Authorize(Policy = "RequireGatewayScope")]
        [HttpGet("gateway-only")]
        public IActionResult GatewayOnly()
        {
            return Ok("You have the 'gateway' scope!");
        }
    }
}
