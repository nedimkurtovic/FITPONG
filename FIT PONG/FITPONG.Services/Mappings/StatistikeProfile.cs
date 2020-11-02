using AutoMapper;

namespace FIT_PONG.Services.Mappings
{
    public class StatistikeProfile:Profile
    {
        public StatistikeProfile()
        {
            CreateMap<Database.DTOs.Statistika, SharedModels.Statistike>();
        }
    }
}
