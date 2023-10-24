using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Todo.Domain.Helpers;
using Todo.Domain.Models;
using Todo.Infrastructure;

namespace Task2_BasicWebApiCRUD.ActionFilters
{
    public class CheckUserAccessActionFilter : IActionFilter
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly AppDbContext _dbContext;
        public CheckUserAccessActionFilter(IHttpContextAccessor contextAccessor, AppDbContext dbContext)
        {

            _contextAccessor = contextAccessor;
            _dbContext = dbContext;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var roles = _contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Role);
            var userId = Guid.Parse(_contextAccessor.HttpContext.User.FindFirstValue("id"));
            if (!roles.Contains(UserRoleType.Admin.ToString()))
            {
                string query = _contextAccessor.HttpContext.Request.Path;
                var queryArray = query.Split('/');
                var itemId = queryArray[queryArray.Length - 1];
                if (!_dbContext.TodoLists.Any(x => x.Id == Guid.Parse(itemId) && x.UserId == userId))
                {
                    context.Result = new ObjectResult(new Response { Message = "Unauthorized request", StatusCode = StatusCodes.Status400BadRequest });
                    return;

                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
