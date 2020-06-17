using FIT_PONG.Services;
using FIT_PONG.Services.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIT_PONG.Database.DTOs;

namespace FIT_PONG.ViewModels.Home
{
    public class HomeIndexVM
    {
        public List<string> ZadnjiRezultati { get; set; }
        public List<(Objava, Takmicenje)> ZadnjeObjave { get; set; } = new List<(Objava, Takmicenje)>();
        public List<TopIgraciVM> TopIgraci { get; set; }
    }
}
