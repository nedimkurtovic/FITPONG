using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Database.DTOs
{
    public class Prijava_igrac
    {
        public Prijava Prijava{ get; set; }
        [Required]
        public int PrijavaID { get; set; }

        public Igrac Igrac { get; set; }
        
        [Required]
        public int IgracID { get; set; }
    }
}
