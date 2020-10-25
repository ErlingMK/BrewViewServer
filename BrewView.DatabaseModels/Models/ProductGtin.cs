using System.ComponentModel.DataAnnotations;

namespace BrewView.DatabaseModels.Models
{
    public class ProductGtin
    {
        [Key] public string ProductId { get; set; }

        public string Gtin { get; set; }
    }
}