using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Domain.Models.Authentication;

namespace Todo.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<IdentityResult> Register(RegisterModel model);
        Task<string> Authenticate(LoginModel model);
    }
}
