using FIT_PONG.Database;
using FIT_PONG.Services.BL;
using FIT_PONG.Services.Services;
using FIT_PONG.Services.Services.Autorizacija;
using FIT_PONG.WebAPI.Services.Bazni;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.WebAPI.Installers
{
    public class DatabaseInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MyDb>(opcije => opcije.UseSqlServer(configuration.GetConnectionString("docker"), b => b.MigrationsAssembly("FITPONG.Database")));
            services.AddIdentity<IdentityUser<int>, IdentityRole<int>>(opcije =>
            {
                opcije.Password.RequiredLength = 6;
                opcije.Password.RequireUppercase = false;
                opcije.Password.RequireNonAlphanumeric = false;
                opcije.Password.RequireDigit = false;
                opcije.SignIn.RequireConfirmedEmail = true;
            })
               .AddEntityFrameworkStores<FIT_PONG.Database.MyDb>()
               .AddDefaultTokenProviders();

            //fina gradska raja
            services.AddScoped<FIT_PONG.Services.BL.InitTakmicenja>();
            services.AddScoped<ISuspenzijaService, SuspenzijaService>();
            services.AddScoped<FIT_PONG.Services.BL.ELOCalculator>();
            services.AddScoped<FIT_PONG.Services.BL.Evidentor>();
            services.AddScoped<FIT_PONG.Services.BL.iEmailServis, FIT_PONG.Services.BL.FITPONGGmail>();
            services.AddScoped<FIT_PONG.Services.BL.TakmicenjeValidator>();

            //ciste puno linija koda, negdje i obraz!
            services.AddScoped<IFeedsService, FeedsService>();
            services.AddScoped<IReportsService, ReportsService>();
            services.AddScoped<IGradoviService, GradoviService>();
            services.AddScoped<IObjaveService, ObjaveService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IStatistikeService, StatistikeService>();
            services.AddScoped<iEmailServis, FITPONGGmail>();
            services.AddScoped<ITakmicenjeService, TakmicenjeService>();
            services.AddScoped<IPrijaveService, PrijaveService>();
            services.AddScoped<IAktivnostiService, AktivnostiService>();

            //combobox jarani
            services.AddScoped<IBaseService<FIT_PONG.SharedModels.KategorijeTakmicenja, object>,
                BaseService<SharedModels.KategorijeTakmicenja, Database.DTOs.Kategorija, object>>();

            services.AddScoped<IBaseService<FIT_PONG.SharedModels.VrsteTakmicenja, object>,
                BaseService<SharedModels.VrsteTakmicenja, Database.DTOs.Vrsta_Takmicenja, object>>();

            services.AddScoped<IBaseService<FIT_PONG.SharedModels.SistemiTakmicenja, object>,
                BaseService<SharedModels.SistemiTakmicenja, Database.DTOs.Sistem_Takmicenja, object>>();

            services.AddScoped<IBaseService<FIT_PONG.SharedModels.StatusiTakmicenja, object>,
                BaseService<SharedModels.StatusiTakmicenja, Database.DTOs.Status_Takmicenja, object>>();

            services.AddScoped<IBaseService<FIT_PONG.SharedModels.VrsteSuspenzija, object>,
                BaseService<SharedModels.VrsteSuspenzija, Database.DTOs.VrstaSuspenzije, object>>();

            //SIPA
            services.AddScoped<ITakmicenjeAutorizator, TakmicenjeAutorizator>();
            services.AddScoped<IUsersAutorizator, UsersAutorizator>();
            services.AddScoped<IGradoviAutorizator, GradoviAutorizator>();
            services.AddScoped<IObjaveAutorizator, ObjaveAutorizator>();
            services.AddScoped<IAktivnostiAutorizator, AktivnostiAutorizator>();

        }
    }
}
