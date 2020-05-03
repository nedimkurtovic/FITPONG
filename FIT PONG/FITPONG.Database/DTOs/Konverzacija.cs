using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Database.DTOs
{
    public class Konverzacija
    {
        public int ID { get; set; }
        public DateTime DatumKreiranja { get; set; }

        public Konverzacija()
        {
            DatumKreiranja = DateTime.Now;
        }
    }
}
