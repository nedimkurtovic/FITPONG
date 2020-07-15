using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.SharedModels
{
    public class Reports
    {
        public int ID { get; set; }
        public string Naslov { get; set; }
        public DateTime DatumKreiranja { get; set; }
        public string Sadrzaj { get; set; }
        public string Email { get; set; }
        public List<Attachmenti> Prilozi { get; set; }
        public List<Fajl> RawPrilozi { get; set; }
    }
}
