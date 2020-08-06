using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIT_PONG.WinForms.PomocniObjekti
{
    class TakmicenjeObjekat
    {
        public int Id { get; set; }
        public string Naziv{ get; set; }
        public int MinELO{ get; set; }
        public string Sistem{ get; set; }
        public string Kategorija{ get; set; }
        public string Vrsta { get; set; }
        public DateTime? DatumPocetka { get; set; }
        public DateTime? DatumZavrsetka { get; set; }
    }
}
