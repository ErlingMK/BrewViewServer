using System.Collections.Generic;

namespace BrewView.Contracts
{
    public class Ingredients
    {
        public IList<Grape> Grapes { get; set; }
        public string Sugar { get; set; }
        public string Acid { get; set; }
    }
}