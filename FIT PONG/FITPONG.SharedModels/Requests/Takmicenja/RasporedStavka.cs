using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.SharedModels.Requests.Takmicenja
{
    public class RasporedStavka
    {
        public string Tim1 { get; set; }
        public string Tim2 { get; set; }
        public int? RezultatTim1{ get; set; }
        public int? RezultatTim2 { get; set; }
    }
}
