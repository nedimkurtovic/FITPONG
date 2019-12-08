using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIT_PONG.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FIT_PONG
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
            services.AddControllersWithViews();
            //ovdje registrujemo dbkontekst(ovo je varijanta bez UoW/Repozitorij patterna odnosno bez ikakvih interfejsa
            //u zavisnosti od baze koju zelis koristiti samo mijenjas string ovdje
            //ova funkcija getconnectionstring je kratica za Configuration["ConnectionStrings:imestringa"] samo sto 
            //ona npr ne dozvoljava da ti se json objekat zove drugacije od ConnectionStrings,da sam ga nazvao KonekcijskiStringovi
            //morao bi koristit ovu alternativnu varijatnu //Configuration["KonekcijskiStringovi:Netza"]
            //konkretno ovaj DI kontenjer services se brine oko kreiranja servisa i disposeanja istih zavisno od njihovog vremena
            //trajanja,postoje scoped transient i singleton
            services.AddDbContext<MyDb>(opcije => opcije.UseSqlServer(Configuration.GetConnectionString("Netza")));
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
