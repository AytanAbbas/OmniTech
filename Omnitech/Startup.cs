using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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

            services.AddScoped<PharmacyInvoiceRepository>();
            services.AddScoped<KNInvoiceRepository>();
            services.AddScoped<SalesLogsRepository>();
            services.AddScoped<PharmacyInvoiceSalesLogsRepository>();
            services.AddScoped<KNInvoiceSalesLogsRepository>();
            services.AddScoped<JobPermissionRepository>(); 
            services.AddScoped<Tps575LogsRepository>();
            services.AddScoped<Tps575UrlRepository>();

            services.AddScoped<KNInvoicePrintService>();
            services.AddScoped<KNInvoiceService>();
            //services.AddSingleton<OmnitechPrintService>();
            services.AddScoped<PharmacyInvoicePrintService>();
            services.AddScoped<PharmacyInvoiceService>();
            services.AddScoped<PrintService>();
            services.AddScoped<PrintTimerService>();

            services.AddScoped<Tps575UrlService>();


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
            app.UseStaticFiles();

            app.UseRouting();
            //

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=ProblemicSalesLogs}/{action=Index}/{id?}");
            });
        }
    }
}
