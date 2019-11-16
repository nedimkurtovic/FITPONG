using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Models
{
    public class Takmicenje
    {
        [Required]
        public int ID { get; set; }
        [Required,StringLength(50)]
        public string Naziv { get; set; }
        public DateTime DatumPocetka{ get; set; }
        public DateTime DatumZavrsetka { get; set; }
        public DateTime RokPocetkaPrijave { get; set; }
        public DateTime RokZavrsetkaPrijave { get; set; }
        public DateTime DatumKreiranja{ get; set; }


        [RegularExpression(@"^[0-9]+$")]
        public int BrojRundi { get; set; }

        [RegularExpression(@"^[0-9]+$")]
        public int MinimalniELO { get; set; }

        public Kategorija Kategorija{ get; set; }
        public int KategorijaID { get; set; }

        public Sistem_Takmicenja Sistem{ get; set; }
        public int SistemID { get; set; }

        public Vrsta_Takmicenja Vrsta { get; set; }
        public int VrstaID { get; set; }

        public Status_Takmicenja Status { get; set; }
        public int StatusID{ get; set; }
    }
}
