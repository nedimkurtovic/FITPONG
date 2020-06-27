using AutoMapper;
using FIT_PONG.SharedModels.Requests.Takmicenja;
using FIT_PONG.ViewModels.TakmicenjeVMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Profiles
{
    public class MapperProfili : Profile
    {
        public MapperProfili()
        {
            CreateMap<SharedModels.Requests.Gradovi.GradoviInsertUpdate, Database.DTOs.Grad>();
            CreateMap<Database.DTOs.Grad,SharedModels.Gradovi>();

            CreateMap<Database.DTOs.Feed, SharedModels.Feeds>();

            CreateMap<SharedModels.Requests.Objave.ObjaveInsertUpdate, Database.DTOs.Objava>();
            CreateMap<Database.DTOs.Objava, SharedModels.Objave>();

            //Webapp mapovi
            CreateMap<EvidencijaMecaVM, EvidencijaMeca>();
            CreateMap<CreateTakmicenjeVM, TakmicenjaInsert>();
        }
    }
}
