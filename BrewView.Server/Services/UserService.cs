using System.Security.Claims;
using BrewView.Server.Services.Abstractions;
using Microsoft.AspNetCore.Http;

namespace BrewView.Server.Services
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

        public string CurrentUser
        {
            get
            {
                var nameId = m_contextAccessor.HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier);
                return nameId != null ? nameId.Value : m_contextAccessor.HttpContext.User.Identity.Name;
            }
        }
    }
}