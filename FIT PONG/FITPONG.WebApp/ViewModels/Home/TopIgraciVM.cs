using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.ViewModels.Home
{
    public class TopIgraciVM
    {
        public int IgracId { get; set; }
        public string Naziv { get; set; }
        public int BrojOsvojenihSetova { get; set; }
        public int BrojPobjeda { get; set; }
        public int BrojPoraza { get; set; }
        public int ELO { get; set; }
    }
}
