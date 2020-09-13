using System.ComponentModel.DataAnnotations;

namespace BrewViewServer.Models.VinmonopolModels
{
    public class Food
    {
        [Key]
        public string FoodId { get; set; }
        public string FoodDesc { get; set; }
    }
}