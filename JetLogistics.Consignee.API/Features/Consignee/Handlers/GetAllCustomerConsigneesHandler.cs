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
        private readonly ILogger<GetAllCustomerConsigneesHandler> _logger;

        public GetAllCustomerConsigneesHandler(IConsigneeService service, ILogger<GetAllCustomerConsigneesHandler> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task<DbReturnModel> HandleAsync(GetAllCustomerConsigneesQuery query)
        {
            try
            {
                var consignees = await _service.GetAllByCustomerIdAsync(query.CustomerId);
                return new DbReturnModel("Consignees fetched successfully", true, consignees.Cast<object>().ToArray());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching consignees for CustomerId {CustomerId}", query.CustomerId);
                return new DbReturnModel("Failed to fetch consignees", false);
            }
        }
    }
}
