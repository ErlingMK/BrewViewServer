namespace BrewView.Contracts
{
    public class Price
    {
        public string PriceValidFrom { get; set; }
        public double? SalesPrice { get; set; }
        public double? SalesPricePrLiter { get; set; }
        public double? BottleReturnValue { get; set; }
    }
}