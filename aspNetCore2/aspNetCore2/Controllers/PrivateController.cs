using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using aspNetCore2.Interfaces;
using aspNetCore2.Models;
using Microsoft.AspNetCore.Antiforgery;
using System.Threading.Tasks;

namespace aspNetCore2.Controllers
{
    public class PrivateController : Controller
    {
        private ISessionService _sessionService;
        private IAntiforgery _antiForgery;

        public PrivateController(ISessionService sessionService, IAntiforgery antiforgery)
        {
            _sessionService = sessionService;
            _antiForgery = antiforgery;
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
        public async Task<IActionResult> Add(string model)
        {
            if (!await _antiForgery.IsRequestValidAsync(Request.HttpContext))
            {
                return new StatusCodeResult(401);
            }
            
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