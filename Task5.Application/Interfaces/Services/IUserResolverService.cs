using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task5.Application.Interfaces.Services
{
    public interface IUserResolverService
    {
        int GetUserId();
        string GetUserName();
        string GetUserRole();
    }
}
