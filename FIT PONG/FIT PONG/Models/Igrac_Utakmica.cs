using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Models
{
    public class Igrac_Utakmica
    {
        [Key]
        public int IgID { get; set; }
        public Igrac Igrac { get; set; }
        public int? IgracID { get; set; }

        public Utakmica Utakmica { get; set; }
        public int UtakmicaID { get; set; }

        public int? PristupniElo { get; set; }
        public int? OsvojeniSetovi { get; set; }

        public Tip_Rezultata TipRezultata{ get; set; }
        public int? TipRezultataID { get; set; }
    }
}
