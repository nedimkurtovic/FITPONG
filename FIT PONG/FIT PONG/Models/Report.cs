using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Models
{
    public class Report
    {
        public int ID { get; set; }
        public string Opis { get; set; }
        public DateTime DatumKreiranja { get; set; }
        public string Sadrzaj { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }

        public Report(string opis, string sadrzaj, int userID)
        {
            Opis = opis;
            DatumKreiranja = DateTime.Now;
            Sadrzaj = sadrzaj;
            UserID = userID;
        }
    }
}
