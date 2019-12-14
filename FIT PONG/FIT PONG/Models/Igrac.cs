using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Models
{
    public class Igrac
    {
        [Key()]
        [ForeignKey(nameof(User))]
        public int ID { get; set; }
        public User User { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage ="Ovo je obavezno polje.")]
        public string PrikaznoIme { get; set; }

        [StringLength(8)]
        public string JacaRuka { get; set; }

        [Range(0,300)]
        public double? Visina { get; set; }
        public int BrojPosjetaNaProfil { get; set; }
        public string ProfileImagePath { get; set; }
        [RegularExpression(@"^[0-9]+$")]
        public int ELO { get; set; }

        public Igrac()
        {
            PrikaznoIme = "NOT SET";
            JacaRuka = "NOT SET";
            Visina = -1;
            BrojPosjetaNaProfil = -1;
            ELO = 1000;
        }



    }
}
