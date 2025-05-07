using JetLogistics.Common.Common;
using JetLogistics.Consignee.API.Features.Consignee.Commands;
using JetLogistics.Consignee.API.Models;
using JetLogistics.Consignee.API.Services;

namespace JetLogistics.Consignee.API.Features.Consignee.Handlers
{
    public class SaveOrUpdateConsigneeHandler : ICommandHandler<SaveOrUpdateConsigneeCommand, string>
    {
        private readonly IConsigneeService _service;

        public SaveOrUpdateConsigneeHandler(IConsigneeService service)
        {
            _service = service;
        }

        public async Task<string> HandleAsync(SaveOrUpdateConsigneeCommand command)
        {
            await _service.SaveOrUpdateAsync(command.Consignee);
            return "Saved or updated successfully.";
        }
    }
}
