using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Database.DTOs
{
    public class Bracket
    {
        public int ID { get; set; }
        [StringLength(110)]
        public string Naziv { get; set; }

        public int TakmicenjeID { get; set; }
        public Takmicenje Takmicenje { get; set; }

        public List<Runda> Runde { get; set; }
    }
}
