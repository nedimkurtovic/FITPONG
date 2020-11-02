using AutoMapper;

namespace FIT_PONG.Services.Mappings
{
    public class TakmicenjeProfile:Profile
    {
        public TakmicenjeProfile()
        {
            CreateMap<Database.DTOs.Takmicenje, SharedModels.Takmicenja>();
            CreateMap<SharedModels.Requests.Takmicenja.TakmicenjaUpdate, Database.DTOs.Takmicenje>();
            CreateMap<SharedModels.Requests.Takmicenja.TakmicenjaInsert, Database.DTOs.Takmicenje>();
        }
    }
}
