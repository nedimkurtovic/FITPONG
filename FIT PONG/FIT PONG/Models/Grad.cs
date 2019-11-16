using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Models
{
    public class Grad
    {
        public int ID { get; set; }
        
        [Required]
        [RegularExpression(@"[a-zA-Z  -]*", ErrorMessage = "Polje smije sadržavati samo slova.")]
        [StringLength(40)]
        public string Naziv { get; set; }

        public Grad()
        { 
            Naziv = "NOT SET"; 
        }

        public Grad(string naziv)
        {
            Naziv = naziv;
        }

    }
}