using JetLogistics.Consignee.API.Models;

namespace JetLogistics.Consignee.API.Services
{
    public class ConsigneeService : IConsigneeService
    {
        public Task SaveOrUpdateAsync(ConsigneeModel consignee)
        {
            // Mock logic: simulate update
            return Task.CompletedTask;
        }
        public Task<List<ConsigneeModel>> GetAllByCustomerIdAsync(int customerId)
        {
            //var data = new List<ConsigneeModel>
            //{
            //    new() { Id = 1, IdCustomer = customerId, Name = "Consignee A", Country = "Japan", Currency = "JPY" },
            //    new() { Id = 2, IdCustomer = customerId, Name = "Consignee B", Country = "USA", Currency = "USD" }
            //};

            //return Task.FromResult(data);

            throw new Exception("Error Test");
        }

        public Task<(object[] Data, int TotalCount)> GetPagedConsigneesAsync(ConsigneeRequestParamModel param)
        {
            var testList = new List<ConsigneeResultModel>
            {
                new ConsigneeResultModel
                {
                    Id = 1,
                    IdCustomer = param.IdCustomer,
                    Name = "Test Consignee 1",
                    Country = "Japan",
                    Currency = "Yen"
                },
                new ConsigneeResultModel
                {
                    Id = 2,
                    IdCustomer = param.IdCustomer,
                    Name = "Test Consignee 2",
                    Country = "USA",
                    Currency = "USD"
                }
            };

            var total = testList.Count;
            var paged = testList.Skip((param.CurrentPage - 1) * param.PageSize).Take(param.PageSize).Cast<object>().ToArray();

            return Task.FromResult((paged, total));
        }
    }
}
