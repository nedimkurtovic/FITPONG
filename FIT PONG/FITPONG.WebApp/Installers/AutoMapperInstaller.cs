using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FIT_PONG.Services.Mappings;
using FIT_PONG.Profiles;
using System.Collections.Generic;

namespace FIT_PONG.WebApp.Installers
{
    public class AutoMapperInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {

            var cfg = new AutoMapper.Configuration.MapperConfigurationExpression();
            cfg.AddProfile<AccountProfile>();
            cfg.AddProfile<BrojKorisnikaLogProfile>();
            cfg.AddProfile<ComboBoxProfile>();
            cfg.AddProfile<FeedsProfile>();
            cfg.AddProfile<GradoviProfile>();
            cfg.AddProfile<ObjaveProfile>();
            cfg.AddProfile<PrijavaProfile>();
            cfg.AddProfile<ReportProfile>();
            cfg.AddProfile<StatistikeProfile>();
            cfg.AddProfile<TakmicenjeProfile>();
            cfg.AddProfile<TakmicenjeVMProfile>();

            var config = new MapperConfiguration(cfg);

            var mapper = config.CreateMapper();

            services.AddSingleton(mapper);
        }
    }
}
