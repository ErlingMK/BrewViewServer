using Microsoft.EntityFrameworkCore;

namespace BrewViewServer.Models.VinmonopolModels
{
    [Owned]
    public class Production
    {
        public string CountryId { get; set; }
        public string Country { get; set; }
        public string RegionId { get; set; }
        public string Region { get; set; }
    }
}