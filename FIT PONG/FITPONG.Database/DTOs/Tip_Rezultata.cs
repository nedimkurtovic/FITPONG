using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Database.DTOs
{
    public class Tip_Rezultata
    {
        [Required]
        public int ID { get; set; }
        [StringLength(50)]
        public string Naziv { get; set; }
    }
}
