using FIT_PONG.Services.Services;
using FIT_PONG.Services.Services.Autorizacija;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.WebApp.Installers
{
    public class DatabaseInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<FIT_PONG.Database.MyDb>(opcije => opcije.UseSqlServer(configuration.GetConnectionString("docker")));

            services.AddIdentity<IdentityUser<int>, IdentityRole<int>>(opcije =>
            {
                opcije.Password.RequiredLength = 6;
                opcije.Password.RequireUppercase = false;
                opcije.Password.RequireDigit = false;
                opcije.SignIn.RequireConfirmedEmail = true;
            })
               .AddEntityFrameworkStores<FIT_PONG.Database.MyDb>()
               .AddDefaultTokenProviders();

            services.AddScoped<FIT_PONG.Services.BL.InitTakmicenja>();
            services.AddScoped<FIT_PONG.Services.BL.TakmicenjeValidator>(); //potrebno testirat
            services.AddScoped<FIT_PONG.Services.BL.ELOCalculator>();
            services.AddScoped<FIT_PONG.Services.BL.Evidentor>();
            services.AddScoped<FIT_PONG.Services.BL.iEmailServis, FIT_PONG.Services.BL.FITPONGGmail>();
            services.AddScoped<ISuspenzijaService, SuspenzijaService>();
            services.AddScoped<ITakmicenjeService, TakmicenjeService>();
            services.AddScoped<IGradoviService, GradoviService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IUsersAutorizator, UsersAutorizator>();


        }
    }
}
