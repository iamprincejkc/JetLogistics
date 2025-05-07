using JetLogistics.Common.Common;
using JetLogistics.Consignee.API.Features.Consignee.Queries;
using JetLogistics.Consignee.API.Models;
using JetLogistics.Consignee.API.Models.Custom;
using JetLogistics.Consignee.API.Services;

namespace JetLogistics.Consignee.API.Features.Consignee.Handlers
{
    public class GetAllCustomerConsigneesHandler : IQueryHandler<GetAllCustomerConsigneesQuery, DbReturnModel>
    {
        private readonly IConsigneeService _service;

        public GetAllCustomerConsigneesHandler(IConsigneeService service)
        {
            _service = service;
        }

        public async Task<DbReturnModel> HandleAsync(GetAllCustomerConsigneesQuery query)
        {
            var consignees = await _service.GetAllByCustomerIdAsync(query.CustomerId);

            return new DbReturnModel("Consignees fetched successfully", true, consignees.Cast<object>().ToArray());
        }
    }
}
