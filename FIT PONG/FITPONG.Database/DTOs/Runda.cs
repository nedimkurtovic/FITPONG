using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Database.DTOs
{
    public class Runda
    {
        [Required]
        public int ID { get; set; }
        [RegularExpression(@"^[0-9]+$")]
        public int BrojRunde { get; set; }
        public DateTime DatumPocetka{ get; set; }

        public Bracket Bracket { get; set; }
        public int BracketID { get; set; }

        public List<Utakmica> Utakmice { get; set; }
    }
}
