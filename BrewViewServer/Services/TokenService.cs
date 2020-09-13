using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BrewViewServer.Models;
using BrewViewServer.Models.User;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BrewViewServer.Services
{
    public class TokenService
    {
        private readonly IConfiguration m_configuration;

        public TokenService(IConfiguration configuration)
        {
            m_configuration = configuration;
        }

        public string CreateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(m_configuration["JwtKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = m_configuration["JwtIssuer"],
                Audience = m_configuration["JwtIssuer"],
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
