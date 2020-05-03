using FIT_PONG.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FIT_PONG.Database.DTOs;

namespace FIT_PONG.ViewModels.IgracVMs
{
    public class IgracVM
    {
        public int ID { get; set; }
        [StringLength(50, ErrorMessage ="Prikazno ime ne smije biti duže od 50 karaktera.")]
        public string PrikaznoIme { get; set; }
        [StringLength(8, ErrorMessage = "Jača ruka ne smije biti duža od 8 karaktera.")]
        public string JacaRuka { get; set; }
        [Range(0, 300, ErrorMessage = "Visina treba biti u rasponu od 1-300.")]
        public double? Visina { get; set; }
        public int BrojPosjetaNaProfil { get; set; }
        public string ProfileImagePath { get; set; }
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "ELO raiting smije sadržavati samo slova.")]
        public int ELO { get; set; }
        public char Spol { get; set; }
        public string Grad { get; set; }
        public Statistika statistika{ get; set; }
        public int BrojPostovanja { get; set; }
        public List<Prijava> listaPrijava{ get; set; }
        public IgracVM(Igrac obj)
        {
            ID = obj.ID;
            PrikaznoIme = obj.PrikaznoIme;
            JacaRuka = obj.JacaRuka;
            Visina = obj.Visina;
            BrojPosjetaNaProfil = obj.BrojPosjetaNaProfil;
            ProfileImagePath = obj.ProfileImagePath;
            ELO = obj.ELO;
            Spol = obj.Spol;
            Grad = obj.Grad!=null?obj.Grad.Naziv:"nije postavljeno";
        }
        public IgracVM(){ }

    }

}
