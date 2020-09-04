using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HotChocolate.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BrewViewServer.Models
{

    public class Brew
    {
        [Key]
        public string ProductId { get; set; }
        public string Gtin { get; set; }
        public IList<AppUserBrew> AppUserBrews { get; set; }

        //public int Id { get; set; }
        //public Basic Basic { get; set; }
        //public Logistics Logistics { get; set; }
        //public Origins Origins { get; set; }
        //public Properties Properties { get; set; }
        //public Classification Classification { get; set; }
        //public Description Description { get; set; }
        //public Ingredients Ingredients { get; set; }
        //public IList<Price> Prices { get; set; }

    }

    public class Note
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NoteId { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
    }


    public class Basic
    {
        [Key]
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

    public class Barcode
    {
        [Key]
        public string Gtin { get; set; }
        public bool IsMainGtin { get; set; }
    }

    public class Logistics
    {
        public Guid? LogisticsId { get; set; }
        public string WholesalerId { get; set; }
        public string WholesalerName { get; set; }
        public string VendorId { get; set; }
        public string VendorName { get; set; }
        public string VendorValidFrom { get; set; }
        public string ManufacturerId { get; set; }
        public string ManufacturerName { get; set; }
        public IList<Barcode> Barcodes { get; set; }
        public string OrderPack { get; set; }
        public double? MinimumOrderQuantity { get; set; }
        public double? PackagingWeight { get; set; }
    }

    [Owned]
    public class Origin
    {
        public string CountryId { get; set; }
        public string Country { get; set; }
        public string RegionId { get; set; }
        public string Region { get; set; }
        public string SubRegionId { get; set; }
        public string SubRegion { get; set; }
    }

    [Owned]
    public class Production
    {
        public string CountryId { get; set; }
        public string Country { get; set; }
        public string RegionId { get; set; }
        public string Region { get; set; }
    }

    public class Origins
    {
        public Guid? OriginsId { get; set; }
        public Origin Origin { get; set; }
        public Production Production { get; set; }
        public string LocalQualityClassifId { get; set; }
        public string LocalQualityClassif { get; set; }
    }

    [Owned]
    public class Properties
    {
        public string EcoLabellingId { get; set; }
        public string EcoLabelling { get; set; }
        public string StoragePotentialId { get; set; }
        public string StoragePotential { get; set; }
        public bool Organic { get; set; }
        public bool Biodynamic { get; set; }
        public bool EthicallyCertified { get; set; }
        public bool VintageControlled { get; set; }
        public bool SweetWine { get; set; }
        public bool FreeOrLowOnGluten { get; set; }
        public bool Kosher { get; set; }
        public bool LocallyProduced { get; set; }
        public bool NoAddedSulphur { get; set; }
        public bool EnvironmentallySmart { get; set; }
        public string ProductionMethodStorage { get; set; }
    }

    [Owned]
    public class Classification
    {
        public string MainProductTypeId { get; set; }
        public string MainProductTypeName { get; set; }
        public string SubProductTypeId { get; set; }
        public string SubProductTypeName { get; set; }
        public string ProductTypeId { get; set; }
        public string ProductTypeName { get; set; }
        public string ProductGroupId { get; set; }
        public string ProductGroupName { get; set; }
    }

    public class Ingredients
    {
        public Guid? IngredientsId { get; set; }
        public IList<Grape> Grapes { get; set; }
        public string Sugar { get; set; }
        public string Acid { get; set; }
    }

    [Owned]
    public class Characteristics
    {
        public string Colour { get; set; }
        public string Odour { get; set; }
        public string Taste { get; set; }
    }

    public class Description
    {
        public Guid? DescriptionId { get; set; }
        public Characteristics Characteristics { get; set; }
        public string Freshness { get; set; }
        public string Fullness { get; set; }
        public string Bitterness { get; set; }
        public string Sweetness { get; set; }
        public string Tannins { get; set; }
        public IList<Food> RecommendedFood { get; set; }
    }

    public class Grape
    {
        public string GrapeId { get; set; }
        public string GrapeDesc { get; set; }
        public string GrapePct { get; set; }
    }

    public class Food
    {
        [Key]
        public string FoodId { get; set; }
        public string FoodDesc { get; set; }
    }

    public class Price
    {
        public Guid? PriceId { get; set; }
        public string PriceValidFrom { get; set; }
        public double? SalesPrice { get; set; }
        public double? SalesPricePrLiter { get; set; }
        public double? BottleReturnValue { get; set; }
    }
}