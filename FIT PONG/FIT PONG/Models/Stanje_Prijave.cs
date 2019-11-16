using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Models
{
    public class Stanje_Prijave
    {
        [Required]
        public int ID { get; set; }

        [RegularExpression(@"^[0-9]+$")]
        public int BrojOdigranihMeceva { get; set; }

        [RegularExpression(@"^[0-9]+$")]
        public int BrojPobjeda { get; set; }
        
        [RegularExpression(@"^[0-9]+$")]
        public int BrojIzgubljenih { get; set; }
        
        [RegularExpression(@"^[0-9]+$")]
        public int BrojBodova { get; set; }

        public int PrijavaID { get; set; }
        public Prijava Prijava{ get; set; }
    }
}
