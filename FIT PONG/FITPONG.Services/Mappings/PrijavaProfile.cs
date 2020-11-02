using AutoMapper;

namespace FIT_PONG.Services.Mappings
{
    public class PrijavaProfile:Profile
    {
        public PrijavaProfile()
        {
            CreateMap<Database.DTOs.Prijava, SharedModels.Prijave>();
            CreateMap<SharedModels.Prijave, Database.DTOs.Prijava>();
        }
    }
}
