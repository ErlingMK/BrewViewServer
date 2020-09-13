using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BrewViewServer.Models.VinmonopolModels
{
    public class AlcoholicEntity
    {
        [Key]
        public string ProductId { get; set; }
        public Basic Basic { get; set; }
        public Logistics Logistics { get; set; }
        public Origins Origins { get; set; }
        public Properties Properties { get; set; }
        public Classification Classification { get; set; }
        public Ingredients Ingredients { get; set; }
        public Description Description { get; set; }
        public IList<Price> Prices { get; set; }
    }
}
