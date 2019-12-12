using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.ViewModels.ReportVMs
{
    public class ReportUniversalVM
    {
        public int ID { get; set; }

        [Required(ErrorMessage="Ovo polje je obavezno")]
        public string Naslov { get; set; }

        public DateTime? DatumKreiranja { get; set; }

        [Required(ErrorMessage = "Ovo polje je obavezno")]
        public string Sadrzaj { get; set; }
        
        [RegularExpression(@"\w+\.\w+@edu\.fit\.ba", ErrorMessage = "Email mora biti u formatu ime.prezime@edu.fit.ba")]
        [Required(ErrorMessage = "Ovo polje je obavezno")]
        public string Email { get; set; }

        public List<IFormFile> Prilozi { get; set; }

    }
}
