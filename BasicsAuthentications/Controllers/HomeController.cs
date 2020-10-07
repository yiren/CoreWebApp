using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BasicsAuthentications.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace BasicsAuthentications.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Authenticate()
        {
            var N5Claims = new List<Claim>(){
                new Claim(ClaimTypes.Name, "n5test"),
                new Claim(ClaimTypes.Email, "n5test@test.com.tw"), 
                new Claim("DeptCode", "1905")
            };

            var n5Identity = new ClaimsIdentity(N5Claims,"n5-memeber");
            var userPrincipal = new ClaimsPrincipal(new[] {n5Identity});

            HttpContext.SignInAsync(userPrincipal);

            return RedirectToAction("Index", new {isAuth="true"});
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
