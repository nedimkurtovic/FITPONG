using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Models
{
    public class TipStatusaPoruke
    {
        public int ID { get; set; }
        
        [Required]
        [StringLength(15)]
        public string Naziv { get; set; }

        public TipStatusaPoruke(string naziv)
        {
            Naziv = naziv;
        }
    }
}
