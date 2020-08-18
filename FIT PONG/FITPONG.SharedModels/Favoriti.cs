using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.SharedModels
{
    public class Favoriti
    {
        public List<(string tim1, int? rez1, int? rez2, string tim2, int utakmicaId)> oznaceneUtakmice { get; set; } = new List<(string tim1, int? rez1, int? rez2, string tim2, int utakmicaId)>();
        public List<(string tim1, int? rez1, int? rez2, string tim2, int utakmicaId)> neoznaceneUtakmice { get; set; } = new List<(string tim1, int? rez1, int? rez2, string tim2, int utakmicaId)>();

    }
}
