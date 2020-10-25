using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OAuthServer.Constants;
//using OAuthServer.Models;

namespace OAuthServer.Controllers
{
    public class OAuthController : Controller
    {
        public OAuthController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Authorize(
            string response_type, // Authorization Flow type
            string client_id,
            string redirect_uri,
            string scope, // resource/info clients want to access
            string state // random 
        )
        {
            var query =  new QueryBuilder();
            query.Add("redirect_uri", redirect_uri);
            query.Add("state", state);

            return View(model:query.ToString());
        }
        [HttpPost]
        public IActionResult Authorize(
            string username,
            string redirect_uri,
            string state
            )
        {
            var code = "AuthCode";
            var query =  new QueryBuilder();
            query.Add("code", code);
            query.Add("state", state);
            return Redirect($"{redirect_uri}{query.ToString()}");
        }

        public async Task<IActionResult> Token(
            string grand_type, // flow of access_token request
            string code, // confirmation of authenication "secret"
            string redirect_uri,
            string client_id
        )
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

            var responseObject = new {
                access_token,
                token_type="Bearer"
            };

            var responseJson = JsonConvert.SerializeObject(responseObject);
            var responseBytes = Encoding.UTF8.GetBytes(responseJson);

            await Response.Body.WriteAsync(responseBytes,0, responseBytes.Length);

            return Redirect(redirect_uri);
        }
        
        
        
    }
}
