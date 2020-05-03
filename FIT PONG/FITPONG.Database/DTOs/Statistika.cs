using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Database.DTOs
{
    public class Statistika
    {
        public int ID { get; set; }
        public int BrojOdigranihMeceva{ get; set; }
        public int BrojSinglePobjeda { get; set; }
        public int BrojOsvojenihTurnira { get; set; }
        public int BrojOsvojenihLiga { get; set; }
        public int NajveciPobjednickiNiz { get; set; }
        public int BrojTimskihPobjeda { get; set; }
        public int AkademskaGodina { get; set; }

        public int IgracID { get; set; }
        public Igrac Igrac { get; set; }

        public Statistika(int igracID)
        {
            BrojOdigranihMeceva = BrojSinglePobjeda = BrojOsvojenihTurnira = BrojOsvojenihLiga = NajveciPobjednickiNiz = BrojTimskihPobjeda = 0;
            AkademskaGodina = DateTime.Now.Year;
            IgracID = igracID;
        }

    }
}
