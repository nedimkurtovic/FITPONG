using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FIT_PONG.SharedModels.Requests.Reports
{
    public class ReportsInsert
    {

        [Required(ErrorMessage = "Ovo polje je obavezno")]
        public string Naslov { get; set; }

        public DateTime? DatumKreiranja { get; set; }

        [Required(ErrorMessage = "Ovo polje je obavezno")]
        public string Sadrzaj { get; set; }

        [RegularExpression(@"\w+\.\w+@edu\.fit\.ba", ErrorMessage = "Email mora biti u formatu ime.prezime@edu.fit.ba")]
        [Required(ErrorMessage = "Ovo polje je obavezno")]
        public string Email { get; set; }

        public List<byte[]> Prilozi { get; set; }
    }
}
