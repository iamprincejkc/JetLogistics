using JetLogistics.Consignee.API.Common;
using JetLogistics.Consignee.API.Models;
using JetLogistics.Consignee.API.Models.Custom;

namespace JetLogistics.Consignee.API.Features.Consignee.Queries
{   
    public class GetCustomerConsigneesNewQuery : IQuery<DbReturnModel>
    {
        public ConsigneeRequestParamModel Params { get; }

        public GetCustomerConsigneesNewQuery(ConsigneeRequestParamModel param)
        {
            Params = param;
        }
    }
}
