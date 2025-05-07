using JetLogistics.Common.Common;
using JetLogistics.Consignee.API.Models.Custom;

namespace JetLogistics.Consignee.API.Features.Consignee.Queries
{
    public class GetAllCustomerConsigneesQuery : IQuery<DbReturnModel>
    {
        public int CustomerId { get; }

        public GetAllCustomerConsigneesQuery(int customerId)
        {
            CustomerId = customerId;
        }
    }
}
