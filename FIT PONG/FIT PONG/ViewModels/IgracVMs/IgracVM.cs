using FIT_PONG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.ViewModels.IgracVMs
{
    public class IgracVM
    {
        public int ID { get; set; }
        public string PrikaznoIme { get; set; }
        public string JacaRuka { get; set; }
        public double Visina { get; set; }
        public int BrojPosjetaNaProfil { get; set; }
        public string ProfileImagePath { get; set; }
        public int ELO { get; set; }
        public IgracVM(Igrac obj)
        {
            ID = obj.ID;
            PrikaznoIme = obj.PrikaznoIme;
            JacaRuka = obj.JacaRuka;
            Visina = obj.Visina;
            BrojPosjetaNaProfil = obj.BrojPosjetaNaProfil;
            ProfileImagePath = obj.ProfileImagePath;
            ELO = obj.ELO;
        }
        public IgracVM(){ }

    }

}
