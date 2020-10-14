using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using BrewView.Contracts.User;
using BrewView.DatabaseModels;
using BrewView.DatabaseModels.User;
using BrewView.Server.Authentication.BrewView;
using BrewView.Server.Models;
using BrewView.Server.Repositories.Abstractions;
using BrewView.Server.Util;
using Microsoft.Extensions.Logging;

namespace BrewView.Server.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IBrewViewAuthentication m_authentication;
        private readonly BrewContext m_brewContext;
        private readonly PasswordHasher m_hasher;
        private readonly ILogger<UserRepository> m_logger;

        public UserRepository(BrewContext brewContext, IBrewViewAuthentication authentication,
            ILogger<UserRepository> logger)
        {
            m_brewContext = brewContext;
            m_authentication = authentication;
            m_logger = logger;
            m_hasher = new PasswordHasher();
        }

        public async Task<UserValidationResponse> CreateOAuthUser(TokenResponse tokenResponse)
        {
            var handler = new JwtSecurityTokenHandler();
            var idToken = handler.ReadJwtToken(tokenResponse.IdToken);

            var user = await Find(idToken.Subject);
            if (user != null)
                return new UserValidationResponse(true, UserValidationResponseMessage.UserExists, tokenResponse.IdToken,
                    tokenResponse.RefreshToken);

            await m_brewContext.Users.AddAsync(new User {Id = idToken.Subject});

            await m_brewContext.SaveChangesAsync();

            return new UserValidationResponse(true, UserValidationResponseMessage.UserCreated, tokenResponse.IdToken,
                tokenResponse.RefreshToken);
        }

        public async Task<UserValidationResponse> Create(string email, string password)
        {
            try
            {
                var user = await Find(email);
                if (user != null) return new UserValidationResponse(false, UserValidationResponseMessage.UserExists);

                var (passwordHash, salt) = m_hasher.Hash(password);

                var entity = await m_brewContext.Users.AddAsync(new User
                    {Id = email, Email = email, PasswordHash = passwordHash, Salt = salt});

                await m_brewContext.SaveChangesAsync();

                return new UserValidationResponse(true, UserValidationResponseMessage.UserCreated,
                    m_authentication.CreateToken(entity.Entity));
            }
            catch (Exception e)
            {
                m_logger.LogError(e, "unable to create user");
                return new UserValidationResponse(false);
            }
        }

        public async Task<UserValidationResponse> SignIn(CredentialsModel credentialsModel)
        {
            try
            {
                var user = await Find(credentialsModel.Email);
                if (user == null)
                    return new UserValidationResponse(false, UserValidationResponseMessage.UserDoesNotExist);

                var result = PasswordHasher.VerifyPassword(credentialsModel, user);

                if (result)
                    return new UserValidationResponse(true, UserValidationResponseMessage.SignedIn,
                        m_authentication.CreateToken(user));

                return new UserValidationResponse(false, UserValidationResponseMessage.InvalidPassword);
            }
            catch (Exception e)
            {
                m_logger.LogError(e, "couldn't sign in");
                return new UserValidationResponse(false);
            }
        }

        public async Task<User> Find(string id)
        {
            return await m_brewContext.Users.FindAsync(id);
        }
    }
}