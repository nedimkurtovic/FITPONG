using FIT_PONG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.ViewModels.TakmicenjeVMs
{
    public class FavoritiVM
    {
        public List<(string tim1, int? rez1, int? rez2, string tim2, int utakmicaId)> oznaceneUtakmice { get; set; } = new List<(string tim1, int? rez1, int? rez2, string tim2, int utakmicaId)>();
        public List<(string tim1, int? rez1, int? rez2, string tim2, int utakmicaId)> neoznaceneUtakmice { get; set; } = new List<(string tim1, int? rez1, int? rez2, string tim2, int utakmicaId)>();
    }
}
