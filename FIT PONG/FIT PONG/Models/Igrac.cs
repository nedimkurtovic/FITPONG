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
        public string PrikaznoIme { get; set; }

        [StringLength(8)]
        public string JacaRuka { get; set; }

        [Range(1,250)]
        public double Visina { get; set; }
        public int BrojPosjetaNaProfil { get; set; }
        public string ProfileImagePath { get; set; }


        public Igrac()
        {
            PrikaznoIme = "NOT SET";
            JacaRuka = "NOT SET";
            Visina = -1;
            BrojPosjetaNaProfil = -1;

        }



    }
}
