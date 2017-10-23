using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using aspNetCore2.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.CodeAnalysis;

namespace aspNetCore2.Controllers
{
    public class PublicController : Controller
    {
        public IActionResult Index() => View();

        [HttpPost]
        public IActionResult Login(LoginViewModel model) {
            if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.Password))
            {
                return new StatusCodeResult(401);
            }

            string hash = string.Empty;
            using (var algorithm = SHA256.Create())
            {
                byte[] input = Encoding.UTF8.GetBytes(model.Password);
                byte[] output = algorithm.ComputeHash(input);
                hash = Convert.ToBase64String(output);
                System.Diagnostics.Debug.WriteLine(hash);
            }
            if (model.Name == "root" && hash == "jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=")
            {
                return RedirectToAction("Index", "Private");
            }
            else
            {
                return new StatusCodeResult(401);
            }
        }
    }
}