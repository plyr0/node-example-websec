using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using aspNetCore2.Interfaces;

namespace aspNetCore2.Controllers
{
    public class PrivateController : Controller
    {
        private ISessionService _sessionService;

        public PrivateController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public IActionResult Index()
        {
            KeyValuePair<string, string>? cookie = Request.Cookies.FirstOrDefault(c => c.Key == "sid");
            if (cookie != null && _sessionService.IsValid(cookie?.Value)) {
                return View();
            } else
            {
                return new StatusCodeResult(401);
            }
            
        }
    }
}