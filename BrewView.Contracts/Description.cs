using System.Collections.Generic;

namespace BrewView.Contracts
{
    public class Description
    {
        public Characteristics Characteristics { get; set; }
        public string Freshness { get; set; }
        public string Fullness { get; set; }
        public string Bitterness { get; set; }
        public string Sweetness { get; set; }
        public string Tannins { get; set; }
        public IList<Food> RecommendedFood { get; set; }
    }
}