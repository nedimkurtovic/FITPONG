using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.SharedModels.Requests.Account
{
    public class SuspenzijaRequest
    {
        public string VrstaSuspenzije { get; set; }
        public DateTime DatumPocetka { get; set; }
        public DateTime DatumZavrsetka { get; set; }
    }
}
