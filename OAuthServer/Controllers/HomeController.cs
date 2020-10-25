using System.Text;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OAuthServer.Models;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using OAuthServer.Constants;
using Microsoft.IdentityModel.Tokens;

namespace OAuthServer.Controllers
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

        public IActionResult Authenicate()
        {
            var claims = new List<Claim>{
              new Claim(JwtRegisteredClaimNames.Sub, "oauth_id"),
              new Claim("DepCode", "1905")  
            };
            

            var secret = Encoding.UTF8.GetBytes(AuthConstants.Secret);

            var key = new SymmetricSecurityKey(secret);

            var alg = SecurityAlgorithms.HmacSha256;

            var signCred =  new SigningCredentials(key, alg);

            var genToken =  new JwtSecurityToken(
                AuthConstants.Issuer,
                AuthConstants.Audience,
                claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddHours(1),
                signCred
                );

            var access_token = new JwtSecurityTokenHandler().WriteToken(genToken);

            return Ok(new {access_token });
        }
        

        public IActionResult Decode(string part)
        {
            var message = Convert.FromBase64String(part);
            var decodedString = Encoding.UTF8.GetString(message);
            return Ok(decodedString);
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
