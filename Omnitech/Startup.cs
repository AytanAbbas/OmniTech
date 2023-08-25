using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Omnitech.Dal.AdoNet;
using Omnitech.Service;

namespace Omnitech
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
            // Add framework services.
            services
                .AddControllersWithViews()
                .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);


            services.AddDistributedMemoryCache();
            services.AddSession(x =>
            {
                x.IdleTimeout = TimeSpan.FromMinutes(15);
            });


            services.AddScoped<PharmacyInvoiceRepository>();
            services.AddScoped<KNInvoiceRepository>();
            services.AddScoped<SalesLogsRepository>();
            services.AddScoped<PharmacyInvoiceSalesLogsRepository>();
            services.AddScoped<KNInvoiceSalesLogsRepository>();
            services.AddScoped<JobPermissionRepository>(); 
            services.AddScoped<Tps575LogsRepository>();
            services.AddScoped<Tps575UrlRepository>();
            services.AddScoped<UserRepository>(); 
            services.AddScoped<UserMenuRepository>(); 


            services.AddScoped<KNInvoicePrintService>();
            services.AddScoped<KNInvoiceService>();
            //services.AddSingleton<OmnitechPrintService>();
            services.AddScoped<PharmacyInvoicePrintService>();
            services.AddScoped<PharmacyInvoiceService>();
            services.AddScoped<PrintService>();
            services.AddScoped<PrintTimerService>();

            services.AddScoped<Tps575UrlService>();



            services.AddScoped<UserManager>();
            services.AddScoped<AuthManager>();
            services.AddScoped<UserMenuManager>();
            services.AddScoped<BaseService>();

            services.AddHttpContextAccessor();

           // services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });



            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Auth/Login";
                    options.LogoutPath = "/Auth/Logout";
                });
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
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            
            app.UseSession();

            app.UseRouting();
            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                     //pattern: "{controller=ProblemicSalesLogs}/{action=Index}/{id?}");
                     pattern: "{controller=Auth}/{action=Login}/{id?}");

            });
        }
    }
}
