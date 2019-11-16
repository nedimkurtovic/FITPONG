using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Models
{
    public class UserUloga
    {
        public int UserID { get; set; }
        public User User { get; set; }

        public int UlogaID { get; set; }
        public Uloga Uloga { get; set; }

        public DateTime DatumDodjele { get; set; }

        public UserUloga(int userID, int ulogaID)
        {
            UserID = userID;
            UlogaID = ulogaID;
            DatumDodjele = DateTime.Now;

        }
    }
}
