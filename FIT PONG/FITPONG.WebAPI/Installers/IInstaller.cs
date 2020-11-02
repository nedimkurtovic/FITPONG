using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FIT_PONG.WebAPI.Installers
{
    public interface IInstaller
    {
        void InstallServices(IServiceCollection services, IConfiguration configuration);
    }
}
