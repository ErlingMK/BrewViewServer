using Microsoft.EntityFrameworkCore;

namespace BrewViewServer.Models.VinmonopolModels
{
    [Owned]
    public class Properties
    {
        public string EcoLabellingId { get; set; }
        public string EcoLabelling { get; set; }
        public string StoragePotentialId { get; set; }
        public string StoragePotential { get; set; }
        public bool Organic { get; set; }
        public bool Biodynamic { get; set; }
        public bool EthicallyCertified { get; set; }
        public bool VintageControlled { get; set; }
        public bool SweetWine { get; set; }
        public bool FreeOrLowOnGluten { get; set; }
        public bool Kosher { get; set; }
        public bool LocallyProduced { get; set; }
        public bool NoAddedSulphur { get; set; }
        public bool EnvironmentallySmart { get; set; }
        public string ProductionMethodStorage { get; set; }
    }
}