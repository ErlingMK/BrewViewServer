using System.Threading.Tasks;
using BrewView.DatabaseModels.User;
using BrewView.Server.Controllers;
using BrewView.Server.Models;
using BrewView.Server.Util;

namespace BrewView.Server.Repositories.Abstractions
{
    public interface IUserRepository
    {
        Task<UserValidationResponse> CreateOAuthUser(TokenResponse tokenResponse);
        Task<UserValidationResponse> Create(string email, string password);
        Task<UserValidationResponse> SignIn(CredentialsModel credentialsModel);
        Task<User> Find(string id);
    }
}