using FIT_PONG.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIT_PONG.Database.DTOs;

namespace FIT_PONG.ViewModels.TakmicenjeVMs 
{ 
    public class TakmicenjeVM
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
        public int MinimalniELO{ get; set; }
        public string Kategorija { get; set; }
        public string Sistem { get; set; }
        public string Vrsta{ get; set; }
        public string Status { get; set; }
        public int BrojPrijavljenih { get; set; }
        public int FeedID { get; set; }
        public bool? Inicirano { get; set; }
        public List<Prijava> Prijave { get; set; }
        public List<Bracket> Bracketi { get; set; }

        public TakmicenjeVM(Takmicenje obj, int brojPrijavljenih = 0)
        {
            ID = obj.ID;
            Naziv = obj.Naziv;
            DatumPocetka = obj.DatumPocetka.GetValueOrDefault();
            DatumZavrsetka = obj.DatumZavrsetka.GetValueOrDefault();
            DatumPocetkaPrijava = obj.RokPocetkaPrijave;
            DatumZavrsetkaPrijava = obj.RokZavrsetkaPrijave;
            Seeded = obj.Seeded;
            DatumKreiranja = obj.DatumKreiranja;
            BrojRundi = obj.BrojRundi.GetValueOrDefault(0);
            MinimalniELO = obj.MinimalniELO;
            Kategorija = obj.Kategorija.Opis;
            Sistem = obj.Sistem.Opis;
            Vrsta = obj.Vrsta.Naziv;
            Status = obj.Status.Opis;
            BrojPrijavljenih = brojPrijavljenih;
            FeedID = obj.FeedID;
            Prijave = obj.Prijave;
            Bracketi= obj.Bracketi;
            Inicirano = obj.Inicirano;
        }
        public TakmicenjeVM() { }
    }
}
