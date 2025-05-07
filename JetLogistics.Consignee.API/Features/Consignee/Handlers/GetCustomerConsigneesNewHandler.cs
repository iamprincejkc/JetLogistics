using JetLogistics.Common.Common;
using JetLogistics.Consignee.API.Features.Consignee.Queries;
using JetLogistics.Consignee.API.Models.Custom;
using JetLogistics.Consignee.API.Services;

namespace JetLogistics.Consignee.API.Features.Consignee.Handlers
{
    public class GetCustomerConsigneesNewHandler : IQueryHandler<GetCustomerConsigneesNewQuery, DbReturnModel>
    {
        private readonly IConsigneeService _service;

        public GetCustomerConsigneesNewHandler(IConsigneeService service)
        {
            _service = service;
        }

        public async Task<DbReturnModel> HandleAsync(GetCustomerConsigneesNewQuery query)
        {
            var (data, totalCount) = await _service.GetPagedConsigneesAsync(query.Params);

            return new DbReturnModel(
                message: "Consignee list fetched",
                success: true,
                data: data,
                count: totalCount,
                currentPage: query.Params.CurrentPage,
                pageSize: query.Params.PageSize,
                currentFirstSize: data.Length > 0 ? 1 : 0,
                currentLastSize: data.Length
            );
        }
    }
}
