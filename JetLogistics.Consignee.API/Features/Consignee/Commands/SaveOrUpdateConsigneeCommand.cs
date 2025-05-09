using JetLogistics.Common.Common;
using JetLogistics.Consignee.API.Models;

namespace JetLogistics.Consignee.API.Features.Consignee.Commands
{
    /// <summary>
    /// Create or update a consignee.
    /// </summary>
    public record SaveOrUpdateConsigneeCommand(ConsigneeModel Consignee) : ICommand<SaveOrUpdateConsigneeCommand>;
}
