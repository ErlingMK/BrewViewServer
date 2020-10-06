using System.ComponentModel.DataAnnotations;

namespace BrewView.DatabaseModels.Vinmonopol
{
    public class Food
    {
        [Key]
        public string FoodId { get; set; }
        public string FoodDesc { get; set; }
    }
}