using System;
using Microsoft.EntityFrameworkCore;

namespace BrewView.DatabaseModels.Vinmonopol
{
    [Owned]
    public class LastChanged
    {
        public DateTime Date { get; set; }
        public string Time { get; set; }
    }
}