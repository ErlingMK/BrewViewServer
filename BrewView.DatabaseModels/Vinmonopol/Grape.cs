using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using BrewView.DatabaseModels.Models;

namespace BrewView.DatabaseModels.Vinmonopol
{
    public class Grape
    {
        public string GrapeId { get; set; }
        public string GrapeDesc { get; set; }

        [NotMapped] public string GrapePct { get; set; }

        public IList<GrapeAlcoholicEntity> GrapeAlcoholicEntities { get; set; }
    }
}