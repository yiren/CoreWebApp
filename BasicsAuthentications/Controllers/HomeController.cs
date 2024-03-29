﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BasicsAuthentications.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BasicsAuthentications.Controllers {
    public class HomeController : Controller {

        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public HomeController (
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager) {
            this.signInManager = signInManager;
            this.userManager = userManager;

        }
        public IActionResult Index () {
            return View ();
        }
        

        [Authorize]
        public IActionResult Privacy () {
            return View ();
        }
        [Authorize(Policy="Claim.Country")]
        public IActionResult ClaimAuth()
        {
            
            return View();
        }
        [Authorize(Roles="Admin")]
        public IActionResult RoleAuth()
        {
            
            return View();
        }
        

        public IActionResult Authenticate () {
            var N5Claims = new List<Claim> () {
                new Claim (ClaimTypes.Name, "n5test"),
                new Claim (ClaimTypes.Email, "n5test@test.com.tw"),
                new Claim ("DeptCode", "1905")
            };

            var n5Identity = new ClaimsIdentity (N5Claims, "n5-memeber");
            var userPrincipal = new ClaimsPrincipal (new [] { n5Identity });

            HttpContext.SignInAsync (userPrincipal);

            return RedirectToAction ("Index", new { isAuth = "true" });
        }

        [ResponseCache (Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error () {
            return View (new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Login () {
            // Alt+P generate view
            return View ();
        }

        [HttpPost]
        public async Task<IActionResult> Login (string username, string password) {
            
            var user = await userManager.FindByNameAsync(username);
            if(user != null){
                await signInManager.SignInAsync(user, isPersistent:false);
                return RedirectToAction("Index");
                // var signInResult = await signInManager.PasswordSignInAsync(username, password, false, false);
                // if(signInResult.Succeeded){
                //     return RedirectToAction ("Index");
                // }
            }
            return RedirectToAction ("Index");
        }

        public async Task<IActionResult> Logout() {
            
            await signInManager.SignOutAsync();
            return RedirectToAction ("Index");
        }

        public IActionResult Register () {
            // Alt+P generate view
            return View ();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password) {
            
            // var user = new IdentityUser(username){
            //     Email=""
            // };
            var userClaims = new List<Claim> () {
                new Claim (ClaimTypes.Name, username),
                new Claim (ClaimTypes.Country, "Taiwan"),
                //new Claim (ClaimTypes.Role, "Admin"),
                new Claim ("DeptCode", "1905")
            };
            
            var user = new IdentityUser(username);
            
            var result = await userManager.CreateAsync(user, password);
            if(result.Succeeded){

                var claimResult =await userManager.AddClaimsAsync(user, userClaims);
                if(claimResult.Succeeded)
                    await signInManager.SignInAsync(user, isPersistent:false);
                return RedirectToAction("Index");
            }
            return RedirectToAction ("Index");
        }

        public async Task<IActionResult> MobileAuthInline(
            [FromServices]IAuthorizationService authorizationService
        )
        {   
            var policyBuilder =  new AuthorizationPolicyBuilder();
            var mobilePolicy = policyBuilder.RequireClaim(ClaimTypes.MobilePhone).Build();
            var authResult = await authorizationService.AuthorizeAsync(User, mobilePolicy);
            if(authResult.Succeeded){
                return View();
            }else{
                return RedirectToAction("Index");
            }
            
        }
        

    }
}