using AutoMapper;
using FIT_PONG.Database.DTOs;
using FIT_PONG.ViewModels.TakmicenjeVMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG
{
    public class AutoMapperProfili: Profile
    {
        public AutoMapperProfili()
        {
            CreateMap<CreateTakmicenjeVM, Takmicenje>();
        }
    }
}
