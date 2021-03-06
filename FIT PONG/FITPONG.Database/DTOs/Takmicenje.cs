﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Database.DTOs
{
    public class Takmicenje
    {
        [Required]
        public int ID { get; set; }
        [Required, StringLength(100)]
        public string Naziv { get; set; }
        public DateTime? RokPocetkaPrijave { get; set; }
        public DateTime? RokZavrsetkaPrijave { get; set; }
        public DateTime DatumKreiranja { get; set; }


        [RegularExpression(@"^[0-9]+$")]
        public int? BrojRundi { get; set; }

        [RegularExpression(@"^[0-9]+$")]
        public int MinimalniELO { get; set; }
        public bool Seeded { get; set; }
        public bool Inicirano { get; set; }
        public Kategorija Kategorija { get; set; }
        public int KategorijaID { get; set; }

        [ForeignKey(nameof(Igrac))]
        public int KreatorID { get; set; }
        public Igrac Kreator{ get; set; }
        public Sistem_Takmicenja Sistem { get; set; }
        public int SistemID { get; set; }

        public Vrsta_Takmicenja Vrsta { get; set; }
        public int VrstaID { get; set; }

        public Status_Takmicenja Status { get; set; }
        public int StatusID { get; set; }
        public DateTime? DatumPocetka { get; set; }
        public DateTime? DatumZavrsetka { get; set; }

        public Feed Feed { get; set; }
        public int FeedID { get; set; }

        public List<Bracket> Bracketi { get; set; }
        public List<Prijava> Prijave { get; set; }

        public void setAtribute(string _naziv, DateTime _pocetakprijava, DateTime _krajprijava,
            int _minimalniELO, int _kategorijaID, int _sistemID, int _vrstaID, int _statusID,
            DateTime? _pocetaktakmicenja, DateTime? _zavrsetakTakmicenja)
        {
            Naziv = _naziv;
            DatumPocetka = _pocetaktakmicenja.GetValueOrDefault();
            DatumZavrsetka = _zavrsetakTakmicenja.GetValueOrDefault();
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
            DateTime? _pocetaktakmicenja=null,DateTime? _zavrsetakTakmicenja=null)
        {
            setAtribute(_naziv, _pocetakprijava, _krajprijava, _minimalniELO, _kategorijaID, _sistemID, _vrstaID
                , _statusID, _pocetaktakmicenja, _zavrsetakTakmicenja);
        }
        //public Takmicenje()//ovdje je bio parametar createtakmicenjeVm obj
        //{
        //    //Naziv = obj.Naziv;
        //    //DatumPocetka = obj.DatumPocetka ?? null;
        //    //DatumZavrsetka = obj.DatumZavrsetka ?? null;
        //    //RokPocetkaPrijave = obj.RokPocetkaPrijave;
        //    //RokZavrsetkaPrijave = obj.RokZavrsetkaPrijave;
        //    //MinimalniELO = obj.MinimalniELO ?? 0;
        //    //KategorijaID = obj.KategorijaID;
        //    //SistemID = obj.SistemID;
        //    //VrstaID = obj.VrstaID;
        //    //StatusID = obj.StatusID;
        //    //DatumKreiranja = DateTime.Now;
        //    //Seeded = obj.Seeded;
        //    BrojRundi = 0;
        //    Bracketi = new List<Bracket>();
        //    Prijave = new List<Prijava>();
        //    Inicirano = false;
        //}
        public Takmicenje()
        {
            Bracketi = new List<Bracket>();
            Prijave = new List<Prijava>();
            Inicirano = false;
            BrojRundi = 0;
        }
    }
}
