using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FIT_PONG.Models;

namespace FIT_PONG.ViewModels.TakmicenjeVMs
{
    public class EditTakmicenjeVM
    {
        public int ID { get; set; }

        public string Naziv { get; set; }
        [Display(Name ="Rok početka prijava")]
        public DateTime? RokPocetkaPrijave { get; set; }
        [Display(Name = "Rok završetka prijava")]
        public DateTime? RokZavrsetkaPrijave { get; set; }

        [Display(Name = "Minimalni ELO")]
        public int? MinimalniELO { get; set; }

        public Kategorija Kategorija { get; set; }
        [Display(Name = "Kategorija")]
        public int KategorijaID { get; set; }

        public Vrsta_Takmicenja Vrsta { get; set; }
        [Display(Name = "Vrsta")]
        public int VrstaID { get; set; }
        public Status_Takmicenja Status { get; set; }
        [Display(Name = "Status")]
        public int StatusID { get; set; }
        [Display(Name = "Datum početka")]
        public DateTime? DatumPocetka { get; set; }
        [Display(Name = "Datum završetka")]
        public DateTime? DatumZavrsetka { get; set; }


        public EditTakmicenjeVM(Takmicenje obj)
        {
            ID = obj.ID;
            Naziv = obj.Naziv;
            DatumPocetka = obj.DatumPocetka ?? null;
            DatumZavrsetka = obj.DatumZavrsetka ?? null;
            RokPocetkaPrijave = obj.RokPocetkaPrijave;
            RokZavrsetkaPrijave = obj.RokZavrsetkaPrijave;
            MinimalniELO = obj.MinimalniELO;
            KategorijaID = obj.KategorijaID;
            VrstaID = obj.VrstaID;
            StatusID = obj.StatusID;
        }
        public EditTakmicenjeVM()
        {

        }
    }
}
