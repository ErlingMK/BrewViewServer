using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BrewViewServer.Models.VinmonopolModels
{
    [Owned]
    public class Ingredients
    {
        public Guid? IngredientsId { get; set; }
        [NotMapped]
        public IList<Grape> Grapes { get; set; }
        public string Sugar { get; set; }
        public string Acid { get; set; }
    }
}