using JetLogistics.Common.Common;
using JetLogistics.Consignee.API.Models;

namespace JetLogistics.Consignee.API.Features.Consignee.Commands
{
    public record SaveOrUpdateConsigneeCommand(ConsigneeModel Consignee) : ICommand<string>;
}
