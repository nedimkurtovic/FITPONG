using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Models
{
    public class Objava
    {
        public int ID { get; set; }
        public string Naziv { get; set; }
        public string Content { get; set; }
        public DateTime DatumKreiranja { get; set; }
        public DateTime DatumIzmjene { get; set; }

    }
}
