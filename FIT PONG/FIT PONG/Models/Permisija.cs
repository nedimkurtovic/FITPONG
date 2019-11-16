using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Models
{
    public class Permisija
    {
        public int ID { get; set; }
        [Required]

        [StringLength(25)]
        public string Naziv { get; set; }

        public Permisija(string naziv)
        {
            Naziv = naziv;
        }
    }
}
