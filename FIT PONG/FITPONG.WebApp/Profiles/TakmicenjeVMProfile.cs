using AutoMapper;
using FIT_PONG.SharedModels.Requests.Takmicenja;
using FIT_PONG.ViewModels.TakmicenjeVMs;

namespace FIT_PONG.Profiles
{
    public class TakmicenjeVMProfile:Profile
    {
        public TakmicenjeVMProfile()
        {
            CreateMap<EvidencijaMecaVM, EvidencijaMeca>()
                .ForMember(x => x.Tim1, s => s.MapFrom(c => c.Tim1))
                .ForMember(x => x.Tim2, s => s.MapFrom(c => c.Tim2))
                .ForMember(x => x.NazivTim1, s => s.MapFrom(c => c.NazivTim1))
                .ForMember(x => x.NazivTim2, s => s.MapFrom(c => c.NazivTim2))
                .ForMember(x => x.RezultatTim1, s => s.MapFrom(c => c.RezultatTim1))
                .ForMember(x => x.RezultatTim2, s => s.MapFrom(c => c.RezultatTim2))
                .ForMember(x=>x.UtakmicaID,opt=>opt.Ignore());
            CreateMap<EvidencijaMeca, EvidencijaMecaVM>();
            CreateMap<CreateTakmicenjeVM, TakmicenjaInsert>();
            CreateMap<EditTakmicenjeVM, TakmicenjaUpdate>();
            CreateMap<CreateTakmicenjeVM, Database.DTOs.Takmicenje>();
 
        }
    }
}
