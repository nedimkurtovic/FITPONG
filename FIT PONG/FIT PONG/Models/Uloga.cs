using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Models
{
    public class Uloga
    {
        public int ID { get; set; }

        [Required]
        [StringLength(20)]
        public string Naziv { get; set; }
        [StringLength(50)]
        public string Opis { get; set; }

        public Uloga(string naziv, string opis)
        {
            Naziv = naziv;
            Opis = opis;
        }
    }
}
