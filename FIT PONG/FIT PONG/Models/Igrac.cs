using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Models
{
    public class Igrac : User
    {
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

        public Igrac(string ime, string prezime, DateTime datumRodjenja, string email, int gradID, int loginID, string prikaznoIme="NOT SET", string jacaRuka="NOT SET", double visina=-1)
            :base(ime,prezime,datumRodjenja,email,gradID,loginID)
        {
            PrikaznoIme = prikaznoIme;
            JacaRuka = jacaRuka;
            Visina = visina;
            BrojPosjetaNaProfil = 0;
        }

    }
}
