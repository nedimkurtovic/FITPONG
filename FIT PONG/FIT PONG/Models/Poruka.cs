using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Models
{
    public class Poruka
    {
        public int ID { get; set; }
        public DateTime DatumVrijeme { get; set; }
        public string Text { get; set; }

        public int KonverzacijaID { get; set; }
        public Konverzacija Konverzacija { get; set; }

        public int TipStatusaPorukeID { get; set; }
        public TipStatusaPoruke TipStatusaPoruke { get; set; }

        public Poruka(string text, int konverzacijaID, int tipStatusaPorukeID)
        {
            DatumVrijeme = DateTime.Now;
            Text = text;
            KonverzacijaID = konverzacijaID;
            TipStatusaPorukeID = tipStatusaPorukeID;
        }
    }
}
