using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FIT_PONG.Services.Mappings;

namespace FIT_PONG.WebAPI.Installers
{
    public class AutoMapperInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(AccountProfile));
            services.AddAutoMapper(typeof(BrojKorisnikaLogProfile));
            services.AddAutoMapper(typeof(ComboBoxProfile));
            services.AddAutoMapper(typeof(FeedsProfile));
            services.AddAutoMapper(typeof(GradoviProfile));
            services.AddAutoMapper(typeof(ObjaveProfile));
            services.AddAutoMapper(typeof(PrijavaProfile));
            services.AddAutoMapper(typeof(ReportProfile));
            services.AddAutoMapper(typeof(StatistikeProfile));
            services.AddAutoMapper(typeof(TakmicenjeProfile));
        }
    }
}
