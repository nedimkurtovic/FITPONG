using FIT_PONG.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FIT_PONG.WebApp.Installers
{
    public class MvcInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithViews(opcije => {
                var policijarukeuvis = new AuthorizationPolicyBuilder()
                                    .RequireAuthenticatedUser()
                                    .Build();
                opcije.Filters.Add(new AuthorizeFilter(policijarukeuvis));
                opcije.Filters.Add<Filterko>(); // exception filter
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
    }
}
