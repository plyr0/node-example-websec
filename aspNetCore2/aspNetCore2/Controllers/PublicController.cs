using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using aspNetCore2.Models;
using System.Security.Cryptography;
using System.Text;
using aspNetCore2.Interfaces;
using Microsoft.AspNetCore.Http;

namespace aspNetCore2.Controllers
{
    public class PublicController : Controller
    {
        private ISessionService _sessionService;

        public PublicController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public IActionResult Index()
        {
            if (Request.Cookies.ContainsKey("sid") && _sessionService.IsValid(Request.Cookies["sid"]))
            {
                return new RedirectToActionResult("Index", "Private", null);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            Guid? logon = _sessionService.Login(model);
            if (logon == null)
            {
                return new StatusCodeResult(401);
            }

            double timeout = double.Parse(Program.Configuration["sessionTimeout"]);
            CookieOptions cookieOptions = new CookieOptions()
            {
                Path = "/",
                Expires = DateTime.Now.AddSeconds(timeout),
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Secure = false
            };
            Response.Cookies.Append("sid", logon.ToString(), cookieOptions);
            return RedirectToAction("Index", "Private");
        }
        
        public IActionResult Twits()
        {
            using (var db = new AppDbContext())
            {
                return View(db.Twits.ToList());
            }
        }
    }
}