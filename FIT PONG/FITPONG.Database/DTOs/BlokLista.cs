using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Database.DTOs
{
    public class BlokLista
    {
        public int IgracID { get; set; }
        public Igrac Igrac { get; set; }
        public int TakmicenjeID { get; set; }
        public Takmicenje Takmicenje { get; set; }
    }
}
