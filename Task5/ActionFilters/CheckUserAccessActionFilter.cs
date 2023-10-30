using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Task5.Application.Interfaces.Repositories;
using Task5.Application.Interfaces.Services;
using Task5.Domain.Entities;
using Task5.Domain.Models;

namespace Task5.ActionFilters
{
    public class CheckUserAccessActionFilter : IActionFilter
    {
        private readonly IUserResolverService _userResolverService;
        private readonly IUserRepository _userRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHttpContextAccessor _httpContext;

        public CheckUserAccessActionFilter(IUserResolverService userResolverService, IUserRepository userRepository, IEmployeeRepository employeeRepository, IHttpContextAccessor httpContext)
        {
            _employeeRepository = employeeRepository;
            _userResolverService = userResolverService;
            _userRepository = userRepository;
            _httpContext = httpContext;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            int userId = _userResolverService.GetUserId();
            string role = _userResolverService.GetUserRole();

            if (!string.Equals(role, Domain.Helpers.Roles.Admin.ToString()))
            {
                string query = _httpContext.HttpContext!.Request.Path;
                var queryArray = query.Split('/');
                int employeeId = int.Parse(queryArray[queryArray.Length - 1]);
                var employee = _employeeRepository.GetEmployeeById(employeeId);
                if (employee != null && employee.UserId != userId)
                {
                    context.Result = new ObjectResult(new Response { Success = false, Message = "Unauthorized request", StatusCode = StatusCodes.Status400BadRequest });
                    return;
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
