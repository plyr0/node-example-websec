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
            if (Request.Cookies.ContainsKey("sid") && _sessionService.IsValid(Request.Cookies["sid"]))
            {
                return View();
            }
            else
            {
                return new StatusCodeResult(401);
            }
        }
    }
}