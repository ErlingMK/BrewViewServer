namespace BrewView.Contracts
{
    public class Basic
    {
        public string ProductId { get; set; }
        public string ProductShortName { get; set; }
        public string ProductLongName { get; set; }
        public double? Volume { get; set; }
        public double? AlcoholContent { get; set; }
        public int? Vintage { get; set; }
        public string AgeLimit { get; set; }
        public string PackagingMaterialId { get; set; }
        public string PackagingMaterial { get; set; }
        public string VolumTypeId { get; set; }
        public string VolumType { get; set; }
        public string CorkTypeId { get; set; }
        public string CorkType { get; set; }
        public int? BottlePerSalesUnit { get; set; }
        public string IntroductionDate { get; set; }
        public string ProductStatusSaleId { get; set; }
        public string ProductStatusSaleName { get; set; }
        public string ProductStatusSaleValidFrom { get; set; }
    }
}