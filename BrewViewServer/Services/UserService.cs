using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace BrewViewServer.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor m_contextAccessor;

        public UserService(IHttpContextAccessor contextAccessor)
        {
            m_contextAccessor = contextAccessor;
        }

        public string GetUserName()
        {
            return m_contextAccessor.HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
        }
    }
}