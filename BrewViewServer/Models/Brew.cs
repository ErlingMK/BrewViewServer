using System.ComponentModel.DataAnnotations;

namespace BrewViewServer.Models
{
    public class Brew
    {
        [Key] public string ProductId { get; set; }

        public string Gtin { get; set; }
    }
}