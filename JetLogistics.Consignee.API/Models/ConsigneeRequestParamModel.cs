namespace JetLogistics.Consignee.API.Models
{
    public class ConsigneeRequestParamModel
    {
        public int IdCustomer { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public string SortBy { get; set; }
        public string OrderBy { get; set; }
        public string Search { get; set; }
    }
}
