using FIT_PONG.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FIT_PONG.Database.DTOs;

namespace FIT_PONG.ViewModels.TakmicenjeVMs
{
    public class EvidencijaMecaVM
    {
        public List<Igrac_Utakmica> Tim1 { get; set; } = new List<Igrac_Utakmica>();
        public List<Igrac_Utakmica> Tim2 { get; set; } = new List<Igrac_Utakmica>();

        public string NazivTim1 { get; set; }
        public string NazivTim2 { get; set; }

        [Required(ErrorMessage ="Morate unijeti rezultat")]
        public int? RezultatTim1 { get; set; }
        [Required(ErrorMessage = "Morate unijeti rezultat")]
        public int? RezultatTim2 { get; set; }
        public int TakmicenjeID { get; set; }
    }
}
