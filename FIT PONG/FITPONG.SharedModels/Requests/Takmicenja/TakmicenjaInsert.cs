using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FIT_PONG.SharedModels.Requests.Takmicenja
{
    public class TakmicenjaInsert
    {
        [Required, StringLength(45, ErrorMessage = "Naziv ne može imati više od 45 karaktera")]
        public string Naziv { get; set; }
        [DisplayName("Pocetak prijava")]
        public DateTime? RokPocetkaPrijave { get; set; }
        [DisplayName("Zavrsetak prijava")]
        //ne smije biti manji od pocetka prijave
        public DateTime? RokZavrsetkaPrijave { get; set; }

        [RegularExpression(@"^[0-9]+$")]
        [Display(Name = "Minimalni ELO")]
        public int? MinimalniELO { get; set; }
        [Display(Name = "Rasporedi igrace na osnovu elo? (prazno oznacava random)")]
        public bool Seeded { get; set; }

        [Display(Name = "Kategorija")]
        public int KategorijaID { get; set; }

        [Display(Name = "Sistem")]
        public int SistemID { get; set; }

        [Display(Name = "Vrsta")]
        public int VrstaID { get; set; }

        //[Display(Name = "Status")]
        //public int StatusID { get; set; }
        [Display(Name = "Rucni unos prijava?(Trenutno samo za singleove dostupno, u suprotnom definisite registracije)")]
        public bool RucniOdabir { get; set; }
        [Display(Name = "Korisnicka imena igraca (format : @KorisnickoIme razmak)")]
        public string RucnoOdabraniIgraci { get; set; }
        [Display(Name = "Datum pocetka")]
        public DateTime? DatumPocetka { get; set; }
        //ne smije biti manji od datuma pocetka
        [Display(Name = "Datum zavrsetka")]
        public DateTime? DatumZavrsetka { get; set; }
    }
}
