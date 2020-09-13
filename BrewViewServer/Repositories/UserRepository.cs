using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using BrewViewServer.Controllers;
using BrewViewServer.Models.User;

namespace BrewViewServer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BrewContext m_brewContext;

        public UserRepository(BrewContext brewContext)
        {
            m_brewContext = brewContext;
        }

        public async Task<User> Create(TokenResponse tokenResponse)
        {
            var handler = new JwtSecurityTokenHandler();
            var idToken = handler.ReadJwtToken(tokenResponse.IdToken);

            var user = await Find(idToken.Subject);
            if (user != null) return user;

            var entry = await m_brewContext.Users.AddAsync(new User {Id = idToken.Subject});

            await m_brewContext.SaveChangesAsync();

            return entry.Entity;
        }

        public async Task<User> Find(string id)
        {
            return await m_brewContext.Users.FindAsync(id);
        }
    }

    public interface IUserRepository
    {
        Task<User> Create(TokenResponse tokenResponse);
        Task<User> Find(string id);
    }
}