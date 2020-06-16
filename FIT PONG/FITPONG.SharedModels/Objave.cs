using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.SharedModels
{
    public class Objave
    {
        public int ID { get; set; }
        public string Naziv { get; set; }
        public string Content { get; set; }
        public DateTime DatumKreiranja { get; set; }
        public DateTime DatumIzmjene { get; set; }
    }
}
