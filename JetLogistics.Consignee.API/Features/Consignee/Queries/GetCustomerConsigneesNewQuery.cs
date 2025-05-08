using JetLogistics.Common.Common;
using JetLogistics.Consignee.API.Models;
using JetLogistics.Consignee.API.Models.Custom;

namespace JetLogistics.Consignee.API.Features.Consignee.Queries
{
    public class GetCustomerConsigneesNewQuery : IQuery<DbReturnModel>
    {
        public ConsigneeRequestParamModel Params { get; }


        /// <summary>
        /// Gets paginated customer consignees by search parameters.
        /// </summary>
        public GetCustomerConsigneesNewQuery(ConsigneeRequestParamModel param)
        {
            Params = param;
        }
    }
}
