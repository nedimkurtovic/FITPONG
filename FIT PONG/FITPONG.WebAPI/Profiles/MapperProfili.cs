using AutoMapper;
using Microsoft.AspNetCore.Identity;
using FIT_PONG.SharedModels.Requests.Takmicenja;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.WebAPI.Profiles
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

            CreateMap<SharedModels.Requests.Account.AccountInsert, SharedModels.Users>();
            CreateMap<Database.DTOs.Igrac, SharedModels.Users>();
            CreateMap<SharedModels.Users, Database.DTOs.Igrac>();


            CreateMap<Database.DTOs.Statistika, SharedModels.Statistike>();


            CreateMap<Database.DTOs.Takmicenje, SharedModels.Takmicenja>();
            CreateMap<SharedModels.Requests.Takmicenja.TakmicenjaUpdate, Database.DTOs.Takmicenje>();
        }
    }
}
