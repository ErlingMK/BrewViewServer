using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrewViewServer.Models
{
    public class UserBrew
    {
        public Brew Brew { get; set; }
        public string ProductId { get; set; }

        public User.User User { get; set; }
        public string UserId { get; set; }

        public IList<Note> Notes { get; set; } = new List<Note>();
        public int Rating { get; set; }
    }
}
