using FIT_PONG.Hubs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FIT_PONG.WebApp.Installers
{
    public class HubInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<NotifikacijeHub>();
        }
    }
}
