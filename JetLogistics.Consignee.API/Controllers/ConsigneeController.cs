using JetLogistics.Common.Common;
using JetLogistics.Consignee.API.Features.Consignee.Commands;
using JetLogistics.Consignee.API.Features.Consignee.Queries;
using JetLogistics.Consignee.API.Models;
using JetLogistics.Consignee.API.Models.Custom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JetLogistics.Consignee.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/consignee")]
    public class ConsigneeController : ControllerBase
    {
        private readonly IDispatcher _dispatcher;

        public ConsigneeController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        /// <summary>
        /// Create or update a consignee.
        /// </summary>
        [HttpPost("save-update")]
        public async Task<IActionResult> SaveUpdateConsignee([FromBody] ConsigneeModel consignee)
        {
            var result = await _dispatcher.DispatchAsync(new SaveOrUpdateConsigneeCommand(consignee));
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("all/{id}")]
        public async Task<ActionResult<DbReturnModel>> GetAllCustomerConsignees(int id)
        {
            var result = await _dispatcher.DispatchAsync(new GetAllCustomerConsigneesQuery(id));
            return Ok(result);
        }

        /// <summary>
        /// Gets paginated customer consignees by search parameters.
        /// </summary>
        [HttpPost("")]
        public async Task<ActionResult<DbReturnModel>> GetCustomerConsigneesNew([FromBody] ConsigneeRequestParamModel param)
        {
            var result = await _dispatcher.DispatchAsync(new GetCustomerConsigneesNewQuery(param));
            return Ok(result);
        }


    }
}
