using Microsoft.EntityFrameworkCore;

namespace BrewViewServer.Models.VinmonopolModels
{
    [Owned]
    public class Characteristics
    {
        public string Colour { get; set; }
        public string Odour { get; set; }
        public string Taste { get; set; }
    }
}