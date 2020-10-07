using Microsoft.AspNetCore.Identity;

namespace BrewView.DatabaseModels.User
{
    public class User
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
    }
}
