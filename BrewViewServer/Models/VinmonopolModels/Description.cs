﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BrewViewServer.Models.VinmonopolModels
{
    [Owned]
    public class Description
    {
        public Guid? DescriptionId { get; set; }
        public Characteristics Characteristics { get; set; }
        public string Freshness { get; set; }
        public string Fullness { get; set; }
        public string Bitterness { get; set; }
        public string Sweetness { get; set; }
        public string Tannins { get; set; }
        [NotMapped]
        public IList<Food> RecommendedFood { get; set; }
    }
}