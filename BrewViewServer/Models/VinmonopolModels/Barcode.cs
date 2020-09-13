using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BrewViewServer.Models.VinmonopolModels
{
    public class Barcode
    {
        [Key]
        public string Gtin { get; set; }
        public bool IsMainGtin { get; set; }
    }
}