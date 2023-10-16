using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Todo.Domain.Helpers;
using Todo.Domain.Models;
using Todo.Infrastructure;

namespace Task2_BasicWebApiCRUD.Middleware
{
    public class CheckUserAccessMiddleware
    {
        private readonly RequestDelegate _next;

        public CheckUserAccessMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, AppDbContext dbContext)
        {
            var roles = context.User.FindFirstValue(ClaimTypes.Role);
            var userId = Guid.Parse(context.User.FindFirstValue("id"));
            if (!roles.Contains(UserRoleType.Admin.ToString()))
            {
                string query = context.Request.Path;
                var queryArray = query.Split('/');
                var itemId = queryArray[queryArray.Length - 1];
                if (!dbContext.TodoLists.Any(x => x.Id == Guid.Parse(itemId) && x.UserId == userId))
                {
                   // context.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsJsonAsync(new Response { Message = "Unauthorized request", StatusCode = (int)System.Net.HttpStatusCode.Unauthorized });
                    return;
                }
            }
            await _next(context);
        }

    }
}
