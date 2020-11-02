using AutoMapper;

namespace FIT_PONG.Services.Mappings
{
    public class BrojKorisnikaLogProfile:Profile
    {
        public BrojKorisnikaLogProfile()
        {
            CreateMap<Database.DTOs.BrojKorisnikaLog, SharedModels.BrojKorisnikaLogs>();
        }
    }
}
