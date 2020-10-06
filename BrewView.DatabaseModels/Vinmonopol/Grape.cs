using System.ComponentModel.DataAnnotations.Schema;

namespace BrewView.DatabaseModels.Vinmonopol
{
    public class Grape
    {
        public string GrapeId { get; set; }
        public string GrapeDesc { get; set; }
        [NotMapped]
        public string GrapePct { get; set; }
    }
}