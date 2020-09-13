using System;
using Microsoft.EntityFrameworkCore;

namespace BrewViewServer.Models.VinmonopolModels
{
    [Owned]
    public class Origins
    {
        public Guid? OriginsId { get; set; }
        public Origin Origin { get; set; }
        public Production Production { get; set; }
        public string LocalQualityClassifId { get; set; }
        public string LocalQualityClassif { get; set; }
    }
}