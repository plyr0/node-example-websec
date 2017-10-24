using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using aspNetCore2.Interfaces;
using aspNetCore2.Models;

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

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Add(string model)
        {
            if (!Request.Cookies.ContainsKey("sid") || !_sessionService.IsValid(Request.Cookies["sid"]))
            {
                return new StatusCodeResult(401);
            }

            var twit = new TwitModel()
            {
                Text = model,
                Time = DateTime.Now,
                Username = _sessionService.GetName(Request.Cookies["sid"])
            };
            using(var db = new AppDbContext()){
                db.Twits.Add(twit);
                db.SaveChanges();
            }
            return RedirectToAction("Twits", "Public");
        }
    }
}