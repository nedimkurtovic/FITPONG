using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Models
{
    public class Takmicenje
    {
        [Required]
        public int ID { get; set; }
        [Required, StringLength(50)]
        public string Naziv { get; set; }
        public DateTime? DatumPocetka { get; set; }
        public DateTime? DatumZavrsetka { get; set; }
        public DateTime RokPocetkaPrijave { get; set; }
        public DateTime RokZavrsetkaPrijave { get; set; }
        public DateTime DatumKreiranja { get; set; }


        [RegularExpression(@"^[0-9]+$")]
        public int? BrojRundi { get; set; }

        [RegularExpression(@"^[0-9]+$")]
        public int MinimalniELO { get; set; }

        public Kategorija Kategorija { get; set; }
        public int KategorijaID { get; set; }

        public Sistem_Takmicenja Sistem { get; set; }
        public int SistemID { get; set; }

        public Vrsta_Takmicenja Vrsta { get; set; }
        public int VrstaID { get; set; }

        public Status_Takmicenja Status { get; set; }
        public int StatusID { get; set; }
        public void setAtribute(string _naziv, DateTime _pocetakprijava, DateTime _krajprijava,
            int _minimalniELO, int _kategorijaID, int _sistemID, int _vrstaID, int _statusID,
            DateTime? _pocetaktakmicenja, DateTime? _zavrsetakTakmicenja = null)
        {
            Naziv = _naziv;
            DatumPocetka = _pocetaktakmicenja;
            DatumZavrsetka = _zavrsetakTakmicenja;
            RokPocetkaPrijave = _pocetakprijava;
            RokZavrsetkaPrijave = _krajprijava;
            MinimalniELO = _minimalniELO;
            KategorijaID = _kategorijaID;
            SistemID = _sistemID;
            VrstaID = _vrstaID;
            StatusID = _statusID;
            DatumKreiranja = DateTime.Now;
            BrojRundi = 0;
        }
        public Takmicenje(string _naziv, DateTime _pocetakprijava, DateTime _krajprijava,
            int _minimalniELO, int _kategorijaID, int _sistemID, int _vrstaID, int _statusID,
            DateTime _pocetaktakmicenja,DateTime? _zavrsetakTakmicenja = null)
        {

            setAtribute(_naziv, _pocetakprijava, _krajprijava, _minimalniELO, _kategorijaID, _sistemID, _vrstaID
                , _statusID, _pocetaktakmicenja, _zavrsetakTakmicenja);
        }
        public Takmicenje()
        {

        }
    }
}
