using System;

namespace BrewView.DatabaseModels.Vinmonopol
{
    public class Price
    {
        public Guid? PriceId { get; set; }
        public string PriceValidFrom { get; set; }
        public double? SalesPrice { get; set; }
        public double? SalesPricePrLiter { get; set; }
        public double? BottleReturnValue { get; set; }
    }
}