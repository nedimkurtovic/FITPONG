using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.SharedModels
{
    public class Prijave
    {
        public int ID { get; set; }
        public bool isTim { get; set; }
        public string Naziv { get; set; }
        public int? Igrac1ID { get; set; }
        public int? Igrac2ID { get; set; }
        public string TakmicenjeNaziv { get; set; }
    }
}
