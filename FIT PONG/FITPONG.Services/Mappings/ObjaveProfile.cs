using AutoMapper;

namespace FIT_PONG.Services.Mappings
{
    public class ObjaveProfile:Profile
    {
        public ObjaveProfile()
        {
            CreateMap<SharedModels.Requests.Objave.ObjaveInsertUpdate, Database.DTOs.Objava>();
            CreateMap<Database.DTOs.Objava, SharedModels.Objave>();
        }
    }
}
