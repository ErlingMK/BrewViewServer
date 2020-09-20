﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BrewViewServer.Models.VinmonopolModels
{
    [Owned]
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
        [NotMapped]
        public IList<Barcode> Barcodes { get; set; }
        public string OrderPack { get; set; }
        public double? MinimumOrderQuantity { get; set; }
        public double? PackagingWeight { get; set; }
    }
}