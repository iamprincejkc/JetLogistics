using JetLogistics.Common.Common;
using JetLogistics.Common.Models;

namespace JetLogistics.Booking.API.Features.Booking.Commands
{
    public class GetAllOriginPortQuery : IQuery<DbReturnModel>
    {
        public int CustomerId { get; }

        public GetAllOriginPortQuery(int customerId)
        {
            CustomerId = customerId;
        }
    }
}
