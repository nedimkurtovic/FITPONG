using AutoMapper;

namespace FIT_PONG.Services.Mappings
{
    public class ComboBoxProfile:Profile
    {
        public ComboBoxProfile()
        {
            CreateMap<Database.DTOs.Kategorija, SharedModels.KategorijeTakmicenja>();
            CreateMap<Database.DTOs.Vrsta_Takmicenja, SharedModels.VrsteTakmicenja>();
            CreateMap<Database.DTOs.Sistem_Takmicenja, SharedModels.SistemiTakmicenja>();
            CreateMap<Database.DTOs.Status_Takmicenja, SharedModels.StatusiTakmicenja>();
            CreateMap<Database.DTOs.VrstaSuspenzije, SharedModels.VrsteSuspenzija>();
        }
    }
}
