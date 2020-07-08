using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.SharedModels
{
    public class Users
    {
        public int ID { get; set; }
        public string PrikaznoIme { get; set; }
        public string JacaRuka { get; set; }
        public double? Visina { get; set; }
        public int BrojPosjetaNaProfil { get; set; }
        //public string ProfileImagePath { get; set; }//ovo ne moze za winforms i xamarin
        public Fajl ProfileImage { get; set; }
        public int ELO { get; set; }
        public char Spol { get; set; }
        public string Grad { get; set; }
        public List<Statistike> statistike { get; set; }
        public int BrojPostovanja { get; set; }
        public List<Prijave> listaPrijava { get; set; }
    }
}
