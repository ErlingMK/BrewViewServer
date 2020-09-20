using BrewViewServer.Models.VinmonopolModels;

namespace BrewViewServer.Models
{
    public class FoodAlcoholicEntity
    {
        public AlcoholicEntity AlcoholicEntity { get; set; }
        public string ProductId { get; set; }


        public Food Food { get; set; }
        public string FoodId { get; set; }
    }
}