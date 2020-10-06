using Microsoft.EntityFrameworkCore;

namespace BrewView.DatabaseModels.Vinmonopol
{
    [Owned]
    public class Characteristics
    {
        public string Colour { get; set; }
        public string Odour { get; set; }
        public string Taste { get; set; }
    }
}