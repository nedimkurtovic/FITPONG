using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Models
{
    public class Status_Takmicenja
    {
        [Required]
        public int ID { get; set; }
        [StringLength(50)]
        public string Opis { get; set; }
    }
}
