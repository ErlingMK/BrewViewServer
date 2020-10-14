namespace BrewView.Contracts
{
    public class Logistics
    {
        public string WholesalerId { get; set; }
        public string WholesalerName { get; set; }
        public string VendorId { get; set; }
        public string VendorName { get; set; }
        public string VendorValidFrom { get; set; }
        public string ManufacturerId { get; set; }
        public string ManufacturerName { get; set; }
        public string OrderPack { get; set; }
        public double? MinimumOrderQuantity { get; set; }
        public double? PackagingWeight { get; set; }
    }
}