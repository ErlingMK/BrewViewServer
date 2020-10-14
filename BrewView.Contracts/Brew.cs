using System.Collections.Generic;

namespace BrewView.Contracts
{
    public class Brew
    {
        public Basic Basic { get; set; }
        public Logistics Logistics { get; set; }
        public Origins Origins { get; set; }
        public Properties Properties { get; set; }
        public Classification Classification { get; set; }
        public Ingredients Ingredients { get; set; }
        public Description Description { get; set; }
        public IList<Price> Prices { get; set; }
        public LastChanged LastChanged { get; set; }
    }
}