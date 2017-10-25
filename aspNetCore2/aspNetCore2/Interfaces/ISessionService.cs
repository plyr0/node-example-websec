using aspNetCore2.Models;
using System;

namespace aspNetCore2.Interfaces
{
    public interface ISessionService
    {
        Guid AddSession(string user);
        bool IsValid(string id);
        string GetName(string id);
        Guid? Login(LoginViewModel model);
    }
}
