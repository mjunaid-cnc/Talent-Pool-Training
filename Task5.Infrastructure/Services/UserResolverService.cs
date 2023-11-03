using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Task5.Application.Interfaces.Services;

namespace Task5.Infrastructure.Services
{
    public class UserResolverService : IUserResolverService
    {
        private readonly IHttpContextAccessor _httpContext;
        public UserResolverService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }
        public int GetUserId()
        {
            return int.Parse(_httpContext.HttpContext!.User.FindFirst("id")!.Value);
        }

        public string GetUserName()
        {
            return _httpContext.HttpContext!.User.FindFirst(ClaimTypes.Name)!.Value;
        }

        public string GetUserRole()
        {
            return _httpContext.HttpContext!.User.FindFirst(ClaimTypes.Role)!.Value;
        }
    }
}
