using JetLogistics.Identity.API.Common;
using Microsoft.AspNetCore.Mvc;

namespace JetLogistics.Identity.API.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IDispatcher _dispatcher;

        public AccountController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        
    }
}
