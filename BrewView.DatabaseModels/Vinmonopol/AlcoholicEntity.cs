using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BrewView.DatabaseModels.Models;

namespace BrewView.DatabaseModels.Vinmonopol
{
    public class AlcoholicEntity
    {
        [Key] public string ProductId { get; set; }

        public Basic Basic { get; set; }
        public Logistics Logistics { get; set; }
        public Origins Origins { get; set; }
        public Properties Properties { get; set; }
        public Classification Classification { get; set; }
        public Ingredients Ingredients { get; set; }
        public Description Description { get; set; }
        public IList<Price> Prices { get; set; }
        public LastChanged LastChanged { get; set; }
        public IList<FoodAlcoholicEntity> FoodAlcoholicEntities { get; set; }
        public IList<GrapeAlcoholicEntity> GrapeAlcoholicEntities { get; set; }
    }
}