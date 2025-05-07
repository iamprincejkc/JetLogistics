namespace JetLogistics.Consignee.API.Models
{
    public class ConsigneeModel
    {
        public int Id { get; set; }
        public int IdCustomer { get; set; }
        public int IdCountry { get; set; }
        public string Country { get; set; }
        public int IdCurrency { get; set; }
        public string Currency { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int IdPOD { get; set; }
        public string POD { get; set; }
        public string UltimateConsignee { get; set; }
        public string UltimateConsigneeAddress { get; set; }
        public int IdPOL { get; set; }
        public string POL { get; set; }
        public string TelNumber { get; set; }
        public string DeliveryAddress { get; set; }
        public string NotifyParty { get; set; }
        public string ContactPerson { get; set; }
        public string Terms { get; set; }
        public string TaxNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Remarks { get; set; }
    }
}
