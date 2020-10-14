using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BrewView.DatabaseModels.Models;

namespace BrewView.DatabaseModels.Vinmonopol
{
    public class Food
    {
        [Key] public string FoodId { get; set; }

        public string FoodDesc { get; set; }
        public IList<FoodAlcoholicEntity> FoodAlcoholicEntities { get; set; }
    }
}