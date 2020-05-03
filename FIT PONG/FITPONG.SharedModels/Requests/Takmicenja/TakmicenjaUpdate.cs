using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FIT_PONG.SharedModels.Requests.Takmicenja
{
    public class TakmicenjaUpdate
    {
        public string Naziv { get; set; }
        [Display(Name = "Rok početka prijava")]
        public DateTime? RokPocetkaPrijave { get; set; }
        [Display(Name = "Rok završetka prijava")]
        public DateTime? RokZavrsetkaPrijave { get; set; }

        [Display(Name = "Minimalni ELO")]
        public int? MinimalniELO { get; set; }

        [Display(Name = "Kategorija")]
        public int KategorijaID { get; set; }

        [Display(Name = "Vrsta")]
        public int VrstaID { get; set; }
        [Display(Name = "Status")]
        public int StatusID { get; set; }
        [Display(Name = "Datum početka")]
        public DateTime? DatumPocetka { get; set; }
        [Display(Name = "Datum završetka")]
        public DateTime? DatumZavrsetka { get; set; }
    }
}
