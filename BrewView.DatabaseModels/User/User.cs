using Microsoft.AspNetCore.Identity;
using System;

namespace BrewView.DatabaseModels.User
{
    public class User
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
