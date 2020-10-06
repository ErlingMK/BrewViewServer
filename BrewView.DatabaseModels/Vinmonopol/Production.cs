using Microsoft.EntityFrameworkCore;

namespace BrewView.DatabaseModels.Vinmonopol
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