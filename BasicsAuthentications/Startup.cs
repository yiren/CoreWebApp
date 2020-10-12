using System.Net;
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
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.InMemory;
using BasicsAuthentications.Data;
using Microsoft.EntityFrameworkCore;

namespace BasicsAuthentications
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

            services.AddDbContext<UserDbContext>(config=> {
                config.UseInMemoryDatabase("InMemoryIdentity");
            });

            services.AddIdentity<IdentityUser, IdentityRole>(config => {
                config.Password.RequireDigit=false;
                config.Password.RequireUppercase=false;
                config.Password.RequireNonAlphanumeric=false;
                config.Password.RequiredLength=4;

            })
                    .AddEntityFrameworkStores<UserDbContext>()
                    .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config=>{
                config.Cookie.Name = "Identity.Cookie";
                config.LoginPath = "/home/login";
                config.LogoutPath = "/home/logout";
            });

            // services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //         .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, config=>{
            //             config.Cookie.Name = "BasicAuthTest";
            //             config.LoginPath ="/Home/Authenticate";
            //         });
            services.AddControllersWithViews();
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
            // 你是誰
            app.UseAuthentication();
            // 你有授權嗎?
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
