using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using BrewView.DatabaseModels;
using BrewView.DatabaseModels.User;
using BrewView.Server.Authentication.BrewView;
using BrewView.Server.Models;
using BrewView.Server.Repositories.Abstractions;
using BrewView.Server.Util;

namespace BrewView.Server.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IBrewViewAuthentication m_authentication;
        private readonly BrewContext m_brewContext;
        private readonly PasswordHasher m_hasher;

        public UserRepository(BrewContext brewContext, IBrewViewAuthentication authentication)
        {
            m_brewContext = brewContext;
            m_authentication = authentication;
            m_hasher = new PasswordHasher();
        }

        public async Task<UserValidationResponse> CreateOAuthUser(TokenResponse tokenResponse)
        {
            var handler = new JwtSecurityTokenHandler();
            var idToken = handler.ReadJwtToken(tokenResponse.IdToken);

            var user = await Find(idToken.Subject);
            if (user != null) return new UserValidationResponse(true, tokenResponse.IdToken, "User already exists", tokenResponse.RefreshToken);

            await m_brewContext.Users.AddAsync(new User {Id = idToken.Subject});

            await m_brewContext.SaveChangesAsync();

            return new UserValidationResponse(true, tokenResponse.IdToken, "User created", tokenResponse.RefreshToken);
        }

        public async Task<UserValidationResponse> Create(string email, string password)
        {
            var user = await Find(email);
            if (user != null) return new UserValidationResponse(true, string.Empty, "User already exists", string.Empty);

            var (passwordHash, salt) = m_hasher.Hash(password);

            var entity = await m_brewContext.Users.AddAsync(new User
                {Id = email, Email = email, PasswordHash = passwordHash, Salt = salt});

            await m_brewContext.SaveChangesAsync();

            return new UserValidationResponse(true, m_authentication.CreateToken(entity.Entity), "User created", string.Empty);
        }

        public async Task<UserValidationResponse> SignIn(CredentialsModel credentialsModel)
        {
            var user = await Find(credentialsModel.Email);
            if (user == null) return new UserValidationResponse(false, string.Empty, "No such user", string.Empty);

            var result = PasswordHasher.VerifyPassword(credentialsModel, user);

            if (result) return new UserValidationResponse(true, m_authentication.CreateToken(user), string.Empty, string.Empty);

            return new UserValidationResponse(false, string.Empty, "Incorrect password", string.Empty);
        }

        public async Task<User> Find(string id)
        {
            return await m_brewContext.Users.FindAsync(id);
        }
    }
}