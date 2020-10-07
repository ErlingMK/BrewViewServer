using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BrewView.DatabaseModels.User;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BrewView.Server.Authentication.BrewView
{
    public class BrewViewAuthentication : IBrewViewAuthentication
    {
        private readonly IConfiguration m_configuration;
        private readonly JwtSecurityTokenHandler m_tokenHandler;

        public BrewViewAuthentication(IConfiguration configuration)
        {
            m_configuration = configuration;
            m_tokenHandler = new JwtSecurityTokenHandler();
        }

        public ClaimsPrincipal ValidateToken(JwtSecurityToken token, string jwtAsString)
        {
            var key = Encoding.ASCII.GetBytes(m_configuration["JwtKey"]);

            var tokenValidationParameters = TokenValidation.TokenValidationParameters;
            tokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(key);

            return m_tokenHandler.ValidateToken(jwtAsString, tokenValidationParameters, out var validatedToken);
        }


        public string CreateToken(User user)
        {
            var key = Encoding.ASCII.GetBytes(m_configuration["JwtKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Id)
                }),
                Expires = DateTime.UtcNow.AddDays(double.Parse(m_configuration["JwtExpireDays"])),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = m_configuration["JwtIssuer"],
                Audience = m_configuration["JwtIssuer"]
            };
            var token = m_tokenHandler.CreateToken(tokenDescriptor);
            return m_tokenHandler.WriteToken(token);
        }
    }
}