using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FIT_PONG.Database;
using FIT_PONG.Services.Services;
using FIT_PONG.Services.Services.Autorizacija;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

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
            services.AddDbContext<MyDb>(opcije => {
                opcije.UseSqlServer(Configuration.GetConnectionString("Netza"),
                    c=>c.MigrationsAssembly("FITPONG.Database"));
                
                });
            services.AddIdentity<IdentityUser<int>,IdentityRole<int>>(opcije=> 
            {
                opcije.Password.RequiredLength = 6;
                opcije.Password.RequireUppercase = false;
                opcije.Password.RequireDigit = false;
                opcije.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<MyDb>()
                .AddDefaultTokenProviders();

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo
                { Version = "v1", Title = "FITPONG.api" });
                x.AddSecurityDefinition("basicAuth", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic"
                });
                x.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "basicAuth" }
                        },
                        new string[]{}
                    }
                });
            });

            services.AddAutoMapper(typeof(Startup));
            
            //fina gradska raja
            services.AddScoped<FIT_PONG.Services.BL.InitTakmicenja>();
            services.AddScoped<FIT_PONG.Services.BL.ELOCalculator>();
            services.AddScoped<FIT_PONG.Services.BL.Evidentor>();
            services.AddScoped<FIT_PONG.Services.BL.iEmailServis, FIT_PONG.Services.BL.FITPONGGmail>();
            services.AddScoped<FIT_PONG.Services.BL.TakmicenjeValidator>();

            //ciste puno linija koda, negdje i obraz!
            services.AddScoped<IFeedsService, FeedsService>();
            services.AddScoped<IGradoviService, GradoviService>();
            services.AddScoped<IObjaveService, ObjaveService>();
            services.AddScoped<ITakmicenjeService, TakmicenjeService>();

            //SIPA
            services.AddScoped<ITakmicenjeAutorizator, TakmicenjeAutorizator>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = "";
            });

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
