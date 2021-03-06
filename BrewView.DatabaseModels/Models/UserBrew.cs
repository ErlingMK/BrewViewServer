﻿using System.Collections.Generic;

namespace BrewView.DatabaseModels.Models
{
    public class UserBrew
    {
        public ProductGtin ProductGtin { get; set; }
        public string ProductId { get; set; }

        public User.User User { get; set; }
        public string UserId { get; set; }

        public IList<Note> Notes { get; set; } = new List<Note>();
        public int Rating { get; set; }

        public int DrunkCount { get; set; }
    }
}
