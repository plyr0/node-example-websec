using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspNetCore2.Interfaces
{
    public interface ISessionService
    {
        Guid AddSession(string user);
        bool IsValid(string id);
    }
}
