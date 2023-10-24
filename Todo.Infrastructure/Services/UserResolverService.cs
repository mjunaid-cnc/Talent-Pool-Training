using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Todo.Application.Interfaces;

namespace Todo.Infrastructure.Services
{
    public class UserResolverService : IUserResolverService
    {
        private readonly IHttpContextAccessor _httpContext;
        public UserResolverService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }
        public Guid GetUserId()
        {
            return Guid.Parse(_httpContext.HttpContext!.User.FindFirstValue("id"));
        }

        public string GetUserName()
        {
            return _httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.Name);
        }

        public string GetUserRole()
        {
            return _httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.Role);
        }
    }
}
