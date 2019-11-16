using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Models
{
    public class UlogaPermisija
    {
        public int UlogaID { get; set; }
        public Uloga Uloga { get; set; }

        public int PermisijaID { get; set; }
        public Permisija Permisija { get; set; }

        public DateTime DatumPostavljanja { get; set; }

        public UlogaPermisija(int ulogaID, int permisijaID)
        {
            UlogaID = ulogaID;
            PermisijaID = permisijaID;
            DatumPostavljanja = DateTime.Now;
        }

    }
}
