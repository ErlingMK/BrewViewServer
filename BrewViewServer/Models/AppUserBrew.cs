using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrewViewServer.Models
{
    public class AppUserBrew
    {
        public Brew Brew { get; set; }
        public string ProductId { get; set; }

        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }

        public IList<Note> Notes { get; set; } = new List<Note>();
        public int Rating { get; set; }
    }
}
