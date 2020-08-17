using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FIT_PONG.SharedModels.Requests.Account
{
    public class AccountInsert
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Morate unijeti email")]
        //[RegularExpression(pattern: "[a-zA-Z0-9.]+@edu\\.fit\\.ba"
        //    , ErrorMessage = "Email mora biti u obliku ime.prezime@edu.fit.ba")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Morate unijeti password")]
        public string Password { get; set; }

        [Display(Name = "Potvrdite password")]
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Morate potvrditi password")]
        [Compare("Password", ErrorMessage = "Morate unijeti isti password")]
        public string PotvrdaPassword { get; set; }
        //_____________________________________ ovo dole je igrac, spajamo sve u jedno

        [StringLength(50, ErrorMessage = "Prikazno ime ne smije biti duže od 50 karaktera.")]
        [RegularExpression(@"[^@ ]*", ErrorMessage = "Prikazno ime ne smije sadržavati karakter @ ili razmak")]
        [Required(ErrorMessage = "Prikazno ime je obavezno.")]
        [Display(Name = "Prikazno ime")]
        public string PrikaznoIme { get; set; }
        [StringLength(8, ErrorMessage = "Jača ruka ne smije biti duža od 8 karaktera.")]
        public string JacaRuka { get; set; }
        [Range(0, 300, ErrorMessage = "Visina treba biti u rasponu od 1-300.")]
        public double? Visina { get; set; }
        public int BrojPosjetaNaProfil { get; set; }
        public Fajl Slika { get; set; }
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "ELO raiting smije sadržavati samo broj.")]
        public int ELO { get; set; }
        public int? GradId { get; set; }
        public char Spol { get; set; }
    }
}
