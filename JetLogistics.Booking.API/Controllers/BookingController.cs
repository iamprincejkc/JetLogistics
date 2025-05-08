using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JetLogistics.Booking.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        [HttpGet("origin/ports")]
        public async Task<IActionResult> GetAllOriginPort()
        {
            return Ok(new { message="Success"});
        }

    }
}
