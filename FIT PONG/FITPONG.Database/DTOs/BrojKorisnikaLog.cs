using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.Database.DTOs
{
    public class BrojKorisnikaLog
    {
        public int ID { get; set; }
        public int BrojKorisnika { get; set; }
        public int MaxBrojKorisnika { get; set; }
        public DateTime Datum { get; set; }
    }
}
