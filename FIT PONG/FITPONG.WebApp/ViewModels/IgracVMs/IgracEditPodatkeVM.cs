using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.ViewModels.IgracVMs
{
    public class IgracEditPodatkeVM
    {
        public int ID { get; set; }
        [StringLength(50, ErrorMessage = "Prikazno ime ne smije biti duže od 50 karaktera.")]
        [Required(ErrorMessage ="Ovo je obavezno polje.")]
        [RegularExpression(@"[^@]*", ErrorMessage = "Prikazno ime ne smije sadržavati karakter @")]
        public string PrikaznoIme { get; set; }
        [StringLength(8)]
        public string JacaRuka { get; set; }
        [Range(0, 300,ErrorMessage ="Visina treba biti u rasponu 1-300.")]
        public double? Visina { get; set; }
        public string ProfileImagePath { get; set; }
        public int? GradId { get; set; }
        public bool TwoFactorEnabled { get; set; }
    }
}
