using System.ComponentModel.DataAnnotations.Schema;

namespace BrewViewServer.Models.VinmonopolModels
{
    public class Grape
    {
        public string GrapeId { get; set; }
        public string GrapeDesc { get; set; }
        [NotMapped]
        public string GrapePct { get; set; }
    }
}