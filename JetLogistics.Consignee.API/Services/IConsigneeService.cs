using JetLogistics.Consignee.API.Models;
using JetLogistics.Consignee.API.Models.Custom;

namespace JetLogistics.Consignee.API.Services
{
    public interface IConsigneeService
    {
        Task SaveOrUpdateAsync(ConsigneeModel consignee);
        Task<List<ConsigneeModel>> GetAllByCustomerIdAsync(int customerId);
        Task<(object[] Data, int TotalCount)> GetPagedConsigneesAsync(ConsigneeRequestParamModel param);
    }
}
