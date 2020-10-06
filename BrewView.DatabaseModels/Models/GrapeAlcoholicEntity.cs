using BrewView.DatabaseModels.Vinmonopol;

namespace BrewView.DatabaseModels.Models
{
    public class GrapeAlcoholicEntity
    {
        public AlcoholicEntity AlcoholicEntity { get; set; }
        public string ProductId { get; set; }


        public Grape Grape { get; set; }
        public string GrapeId { get; set; }

        public string GrapePercent { get; set; }
    }
}