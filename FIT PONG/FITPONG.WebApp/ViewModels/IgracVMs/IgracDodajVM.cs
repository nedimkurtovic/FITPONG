using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.ViewModels.IgracVMs
{
    public class IgracDodajVM
    {
        public int? ID { get; set; }
        public string Token { get; set; }
        [StringLength(50, ErrorMessage = "Prikazno ime ne smije biti duže od 50 karaktera.")]
        [RegularExpression(@"[^@ ]*",ErrorMessage = "Prikazno ime ne smije sadržavati karakter @ ili razmak")]
        [Required(ErrorMessage ="Prikazno ime je obavezno.")]
        [Display(Name ="Prikazno ime")]
        public string PrikaznoIme { get; set; }
        [StringLength(8, ErrorMessage = "Jača ruka ne smije biti duža od 8 karaktera.")]
        public string JacaRuka { get; set; }
        [Range(0, 300, ErrorMessage = "Visina treba biti u rasponu od 1-300.")]
        public double? Visina { get; set; }
        public int BrojPosjetaNaProfil { get; set; }
        public IFormFile Slika { get; set; }
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "ELO raiting smije sadržavati samo broj.")]
        public int ELO { get; set; }
        public int? GradId { get; set; }
        public char Spol { get; set; }

    }
}
