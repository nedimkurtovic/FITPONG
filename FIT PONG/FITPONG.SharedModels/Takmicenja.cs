using FIT_PONG.Database.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.SharedModels
{
    public class Takmicenja
    {
        public int ID { get; set; }
        public string Naziv { get; set; }
        public DateTime? DatumPocetka { get; set; }
        public DateTime? DatumZavrsetka { get; set; }
        public DateTime? DatumPocetkaPrijava { get; set; }
        public DateTime? DatumZavrsetkaPrijava { get; set; }
        public DateTime DatumKreiranja { get; set; }
        public int BrojRundi { get; set; }
        public bool Seeded { get; set; }
        public int MinimalniELO { get; set; }
        public string Kategorija { get; set; }
        public string Sistem { get; set; }
        public string Vrsta { get; set; }
        public string Status { get; set; }
        public int BrojPrijavljenih { get; set; }
        public int FeedID { get; set; }
        public int KreatorID { get; set; }
        public bool? Inicirano { get; set; }
        public List<Prijava> Prijave { get; set; }
        public List<Bracket> Bracketi { get; set; }
    }
}
