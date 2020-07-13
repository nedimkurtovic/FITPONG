using FIT_PONG.Database.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FIT_PONG.Database.DTOs
{
    public class Suspenzija
    {
        public int ID { get; set; }

        public int IgracID{ get; set; }
        public Igrac Igrac { get; set; }

        public int VrstaSuspenzijeID { get; set; }
        public VrstaSuspenzije VrstaSuspenzije{ get; set; }

        public DateTime DatumPocetka { get; set; }
        public DateTime DatumZavrsetka { get; set; }
        
    }
}
