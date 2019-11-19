using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIT_PONG.Models;

namespace FIT_PONG.ViewModels.TakmicenjeVMs
{
    public class EditTakmicenjeVM
    {
        public int ID { get; set; }

        public string Naziv { get; set; }
        public DateTime RokPocetkaPrijave { get; set; }
        public DateTime RokZavrsetkaPrijave { get; set; }

        public int MinimalniELO { get; set; }

        public Kategorija Kategorija { get; set; }
        public int KategorijaID { get; set; }

        public Vrsta_Takmicenja Vrsta { get; set; }
        public int VrstaID { get; set; }

        public Status_Takmicenja Status { get; set; }
        public int StatusID { get; set; }
        public DateTime? DatumPocetka { get; set; }
        public DateTime? DatumZavrsetka { get; set; }

        public EditTakmicenjeVM(Takmicenje obj)
        {
            ID = obj.ID;
            Naziv = obj.Naziv;
            DatumPocetka = obj.DatumPocetka.GetValueOrDefault();
            DatumZavrsetka = obj.DatumZavrsetka.GetValueOrDefault();
            RokPocetkaPrijave = obj.RokPocetkaPrijave;
            RokZavrsetkaPrijave = obj.RokZavrsetkaPrijave;
            MinimalniELO = obj.MinimalniELO;
            KategorijaID = obj.Kategorija.ID;
            VrstaID = obj.Vrsta.ID;
            StatusID = obj.Status.ID;
        }
    }
}
