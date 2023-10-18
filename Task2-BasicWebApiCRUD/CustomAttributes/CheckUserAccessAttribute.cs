using System.Security.Claims;
using Todo.Domain.Helpers;
using Todo.Domain.Models;
using Todo.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace Task2_BasicWebApiCRUD.CustomAttributes
{

    public class CheckUserAccessAttribute : TypeFilterAttribute
    {
        public CheckUserAccessAttribute() : base(typeof(CheckUserAccessFilter))
        {
        }
    }

    public class CheckUserAccessFilter : IAsyncActionFilter
    {
        private readonly AppDbContext _dbContext;

        public CheckUserAccessFilter(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var roles = context.HttpContext.User.FindFirstValue(ClaimTypes.Role);
            var userId = Guid.Parse(context.HttpContext.User.FindFirstValue("id"));

            if (!roles.Contains(UserRoleType.Admin.ToString()))
            {
                var itemId = Guid.Empty;
                if (context.RouteData.Values.TryGetValue("id", out var idValue))
                {
                    if (Guid.TryParse(idValue.ToString(), out var parsedId))
                    {
                        itemId = parsedId;
                    }
                }

                if (itemId != Guid.Empty && !await _dbContext.TodoLists.AnyAsync(x => x.Id == itemId && x.UserId == userId))
                {
                    context.Result = new ObjectResult(new Response { Message = "Unauthorized request", StatusCode = (int)System.Net.HttpStatusCode.Unauthorized })
                    {
                        StatusCode = StatusCodes.Status401Unauthorized
                    };
                    return;
                }
            }

            await next();
        }
    }

}
