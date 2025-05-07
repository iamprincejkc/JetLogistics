using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JetLogistics.Consignee.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/consignee/test")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                Message = "Consignee API is authenticated and working.",
                User = User.Identity?.Name,
                Claims = User.Claims.Select(c => new { c.Type, c.Value })
            });
        }
    }
}
