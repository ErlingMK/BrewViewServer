using System.ComponentModel.DataAnnotations;

namespace BrewView.DatabaseModels.Vinmonopol
{
    public class Barcode
    {
        [Key]
        public string Gtin { get; set; }
        public bool IsMainGtin { get; set; }
    }
}