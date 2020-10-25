using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace OAuthMVCClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // using Microsoft.AspNetCore.Authentication.Cookies;
            services.AddAuthentication(option => {
                 option.DefaultAuthenticateScheme = "MVC_Client_Cookie";
                 option.DefaultSignInScheme = "MVC_Client_Cookie";
                 option.DefaultChallengeScheme = "AuthServer";
            })
                .AddCookie("MVC_Client_Cookie")
                .AddOAuth("AuthServer", option =>{
                    option.CallbackPath ="/oauth/callback";
                    option.ClientId = "MVC_Client";
                    option.ClientSecret = "MVC_Secret";
                    option.AuthorizationEndpoint = "https://localhost:5050/oauth/authorize";
                    option.TokenEndpoint = "https://localhost:5050/oauth/token";
                    option.SaveTokens = true;
                });
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
