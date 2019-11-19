using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FIT_PONG.Models;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.ViewModels.TakmicenjeVMs
{
    public class CreateTakmicenjeVM
    {
        [Required]
        public int ID { get; set; }
        [Required, StringLength(50)]
        public string Naziv { get; set; }
        [DisplayName("Pocetak prijava")]
        public DateTime RokPocetkaPrijave { get; set; }
        [DisplayName("Zavrsetak prijava")]
        //ne smije biti manji od pocetka prijave
        public DateTime RokZavrsetkaPrijave { get; set; }

        [RegularExpression(@"^[0-9]+$")]
        public int MinimalniELO { get; set; }

        public int KategorijaID { get; set; }
        public int SistemID { get; set; }
        public int VrstaID { get; set; }
        public int StatusID { get; set; }

        public DateTime? DatumPocetka { get; set; }
        //ne smije biti manji od datuma pocetka
        public DateTime? DatumZavrsetka { get; set; }
    }
}
