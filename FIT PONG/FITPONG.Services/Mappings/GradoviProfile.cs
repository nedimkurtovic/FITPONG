using AutoMapper;

namespace FIT_PONG.Services.Mappings
{
    public class GradoviProfile: Profile
    {
        public GradoviProfile()
        {
            CreateMap<SharedModels.Requests.Gradovi.GradoviInsertUpdate, Database.DTOs.Grad>();
            CreateMap<Database.DTOs.Grad, SharedModels.Gradovi>();
        }
    }
}
