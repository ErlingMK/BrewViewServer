﻿using System;
using Microsoft.EntityFrameworkCore;

namespace BrewView.DatabaseModels.Vinmonopol
{
    [Owned]
    public class Origins
    {
        public Origin Origin { get; set; }
        public Production Production { get; set; }
        public string LocalQualityClassifId { get; set; }
        public string LocalQualityClassif { get; set; }
    }
}