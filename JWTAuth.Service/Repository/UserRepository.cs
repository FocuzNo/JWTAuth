using JWTAuth.Service.IRepository;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace JWTAuth.Service.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetName()
        {
            var name = string.Empty;
            
            if(_httpContextAccessor.HttpContext is not null)
            {
                name = _httpContextAccessor.HttpContext.User?
                    .FindFirstValue(ClaimTypes.Name);
            }

            return name!;
        }
    }
}
