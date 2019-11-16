using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Models
{
    public class Prijava
    {
        [Required]
        public int ID { get; set; }
        public DateTime DatumPrijave{ get; set; }
        public bool isTim { get; set; }
        [StringLength(50)]
        public string Naziv { get; set; }

        //public int StanjePrijaveID { get; set; }
        public Stanje_Prijave StanjePrijave{ get; set; }

        public int TakmicenjeID { get; set; }
        public Takmicenje Takmicenje{ get; set; }
    }
}
