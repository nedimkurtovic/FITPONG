using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FIT_PONG.Database;
using FIT_PONG.Services.BL;
using FIT_PONG.Services.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FITPONG.WebAPI
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
            services.AddControllers();
            services.AddAutoMapper(typeof(Startup));
            services.AddDbContext<FIT_PONG.Database.MyDb>(opcije => opcije.UseSqlServer(Configuration.GetConnectionString("Plesk")));

            services.AddIdentity<IdentityUser<int>,IdentityRole<int>>(opcije=> 
            {
                opcije.Password.RequiredLength = 6;
                opcije.Password.RequireUppercase = false;
                opcije.Password.RequireDigit = false;
                opcije.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<MyDb>()
                .AddDefaultTokenProviders();

            services.AddScoped<FIT_PONG.Services.Services.IFeedsService, FIT_PONG.Services.Services.FeedsService>();
            services.AddScoped<FIT_PONG.Services.Services.IGradoviService, FIT_PONG.Services.Services.GradoviService>();
            services.AddScoped<FIT_PONG.Services.Services.IObjaveService, FIT_PONG.Services.Services.ObjaveService>();
            services.AddScoped<FIT_PONG.Services.Services.IUsersService, FIT_PONG.Services.Services.UsersService>();
            services.AddScoped<FIT_PONG.Services.Services.IStatistikeService, FIT_PONG.Services.Services.StatistikeService>();
            services.AddScoped<FIT_PONG.Services.BL.iEmailServis, FIT_PONG.Services.BL.FITPONGGmail>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
