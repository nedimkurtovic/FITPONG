using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Database.DTOs
{
    public class UserKonverzacija
    {
        public int UserID { get; set; }
        public IdentityUser<int> User { get; set; }

        public int KonverzacijaID { get; set; }
        public Konverzacija Konverzacija { get; set; }

        public UserKonverzacija(int userID, int konverzacijaID)
        {
            UserID = userID;
            KonverzacijaID = konverzacijaID;
        }

    }
}
