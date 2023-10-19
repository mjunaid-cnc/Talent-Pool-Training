using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Application.Interfaces
{
    public interface IUserResolverService
    {
        Guid GetUserId();
        string GetUserName();
        string GetUserRole();

    }
}
