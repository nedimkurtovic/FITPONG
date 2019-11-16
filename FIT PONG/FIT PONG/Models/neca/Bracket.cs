using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Models
{
    public class Bracket
    {
        public int ID { get; set; }
        [StringLength(40)]
        public string Naziv { get; set; }
    }
}
