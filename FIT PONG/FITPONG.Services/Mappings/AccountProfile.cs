using AutoMapper;

namespace FIT_PONG.Services.Mappings
{
    public class AccountProfile: Profile
    {
        public AccountProfile()
        {
            CreateMap<SharedModels.Requests.Account.AccountInsert, SharedModels.Users>();
            CreateMap<Database.DTOs.Igrac, SharedModels.Users>();
            CreateMap<SharedModels.Users, Database.DTOs.Igrac>();
        }

    }
}
