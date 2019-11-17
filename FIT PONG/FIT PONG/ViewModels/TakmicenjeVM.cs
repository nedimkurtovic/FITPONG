using FIT_PONG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.ViewModels
{
    public class TakmicenjeVM
    {
        public int ID { get; set; }
        public string Naziv { get; set; }
        public DateTime DatumPocetka { get; set; }
        public DateTime DatumZavrsetka { get; set; }
        public DateTime DatumPocetkaPrijava { get; set; }
        public DateTime DatumZavrsetkaPrijava { get; set; }
        public DateTime DatumKreiranja { get; set; }
        public int BrojRundi { get; set; }
        public int MinimalniELO{ get; set; }
        public string Kategorija { get; set; }
        public string Sistem { get; set; }
        public string Vrsta{ get; set; }
        public string Status { get; set; }
        public int BrojPrijavljenih { get; set; }

        public TakmicenjeVM(Takmicenje obj, int brojPrijavljenih = 0)
        {
            ID = obj.ID;
            Naziv = obj.Naziv;
            DatumPocetka = obj.DatumPocetka.GetValueOrDefault(DateTime.Parse("1 Jan 1900"));
            DatumZavrsetka = obj.DatumZavrsetka.GetValueOrDefault(DateTime.Parse("1 Jan 1900"));
            DatumPocetkaPrijava = obj.RokPocetkaPrijave;
            DatumZavrsetkaPrijava = obj.RokZavrsetkaPrijave;
            DatumKreiranja = obj.DatumKreiranja;
            BrojRundi = obj.BrojRundi.GetValueOrDefault(0);
            MinimalniELO = obj.MinimalniELO;
            Kategorija = obj.Kategorija.Opis;
            Sistem = obj.Sistem.Opis;
            Vrsta = obj.Vrsta.Naziv;
            Status = obj.Status.Opis;
            BrojPrijavljenih = brojPrijavljenih;
        }
    }
}
