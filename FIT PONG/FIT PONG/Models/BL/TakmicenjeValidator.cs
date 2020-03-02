using FIT_PONG.ViewModels.TakmicenjeVMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FIT_PONG.Models.BL
{
    public class TakmicenjeValidator
    {
        public List<string> _listaTakmicenja { get; set; } = new List<string>();
        public List<Igrac> _listaIgraca { get; set; } = new List<Igrac>();

        public List<(string key, string error)> VratiListuErroraAkcijaDodaj(CreateTakmicenjeVM objekat,
            List<string> ListaTakmicenja, List<Igrac> ListaIgraca)
        {
            _listaTakmicenja = ListaTakmicenja;
            _listaIgraca = ListaIgraca;

            List<(string key, string error)> listaErrora = new List<(string key, string error)>();
            if (PostojiTakmicenje(objekat.Naziv))
                listaErrora.Add(("", "Vec postoji takmicenje u bazi"));
            if (!objekat.RucniOdabir)
            {
                if (objekat.RokZavrsetkaPrijave != null && objekat.RokZavrsetkaPrijave != null &&
                  objekat.RokZavrsetkaPrijave < objekat.RokPocetkaPrijave)
                    listaErrora.Add((nameof(objekat.RokZavrsetkaPrijave), "Datum zavrsetka prijava ne moze biti prije pocetka"));
                if (objekat.DatumPocetka != null && objekat.RokZavrsetkaPrijave != null && objekat.DatumPocetka < objekat.RokZavrsetkaPrijave)
                    listaErrora.Add((nameof(objekat.DatumPocetka), "Datum pocetka ne moze biti prije zavrsetka prijava"));
            }
            else
            {
                //u slucaju da ljudi nisu dodali razmake ili ih je viska da ja popravim situaciju malo
                if (objekat.RucnoOdabraniIgraci != null && objekat.RucnoOdabraniIgraci != "")
                {
                    if (objekat.RucnoOdabraniIgraci.EndsWith(" "))
                        objekat.RucnoOdabraniIgraci = objekat.RucnoOdabraniIgraci.Remove(objekat.RucnoOdabraniIgraci.Length - 1);
                    if (objekat.RucnoOdabraniIgraci.StartsWith(" "))
                        objekat.RucnoOdabraniIgraci = objekat.RucnoOdabraniIgraci.Substring(1);
                    //za sad je hardkodirana vrsta(predstavlja uslov da ne mogu doubleovi u rucnom unosu),ovo ionako ne bi trebalo nikad biti true osim ako je neko zaobisao frontend
                    if (objekat.VrstaID == 2 ||
                        objekat.RucnoOdabraniIgraci == "" ||
                        !ValidanUnosRegex(objekat.RucnoOdabraniIgraci) ||
                        !ValidnaKorisnickaImena(objekat.RucnoOdabraniIgraci)
                        )
                    {
                        listaErrora.Add(("", "Molimo unesite ispravno imena igraca"));
                    }
                    if (RucnaImenaSadrziDuplikate(objekat.RucnoOdabraniIgraci))
                        listaErrora.Add(("", "Nemojte dva puta istog igraca navoditi"));
                    if (BrojRucnoUnesenih(objekat.RucnoOdabraniIgraci) < 4)
                        listaErrora.Add(("", "Minimalno 4 igraca za takmicenje"));
                }
                else
                {
                    listaErrora.Add(("", "Molimo unesite ispravno imena igraca"));
                }
            }
            _listaTakmicenja = null;
            _listaIgraca = null;
            return listaErrora;
        }
        private bool PostojiTakmicenje(string naziv)
        {
            if (_listaTakmicenja.Where(s => s == naziv).Count() > 0)
                return true;
            return false;
        }
        private bool ValidanUnosRegex(string ProslijedjenaImena)
        {
            //Regex pattern = new Regex("\\B@.[^@ ]+");
            var match = Regex.Matches(ProslijedjenaImena, "(?:^|\\s)(?<username>[^\\s]*)(?=\\s|$)");
            int brojUkupnoMatcheva = match.Count();
            if (brojUkupnoMatcheva == 0)
                return false;

            foreach (Match x in match)
            {
                if (x.Success)
                {
                    if (x.Groups["username"].Length == 0)
                        return false;
                }
            }
            return true;

            //return sumamatcheva == ProslijedjenaImena.Count();
        }
        private bool ValidnaKorisnickaImena(string proslijedjenaImena)
        {
            var matches = Regex.Matches(proslijedjenaImena, "(?:^|\\s)(?<username>[^\\s]*)(?=\\s|$)");
            foreach (Match i in matches)
            {
                string KorisnickoIme = i.Groups["username"].Value;
                if (_listaIgraca.Where(x => x.PrikaznoIme == KorisnickoIme).Count() == 0)
                    return false;
            }
            return true;
        }
        private bool RucnaImenaSadrziDuplikate(string ProslijedjenaImena)//ako je proslijedio 2 puta istog frajera
        {
            var matches = Regex.Matches(ProslijedjenaImena, "(?:^|\\s)(?<username>[^\\s]*)(?=\\s|$)");// rezultati su u prvoj grupi
            List<string> svePrijave = new List<string>();
            foreach (Match i in matches)
            {
                string KorisnickoIme = i.Groups["username"].Value;
                if (svePrijave.Contains(KorisnickoIme))
                    return true;
                svePrijave.Add(KorisnickoIme);
            }
            return false;
        }
        private int BrojRucnoUnesenih(string proslijedjenaImena)
        {
            var matches = Regex.Matches(proslijedjenaImena, "(?:^|\\s)(?<username>[^\\s]*)(?=\\s|$)");
            int BrojKorisnika = 0;
            foreach (Match x in matches)
            {
                if (x.Groups["username"].Success)
                    BrojKorisnika++;
            }
            return BrojKorisnika;
        }

        public List<Igrac> GetListaRucnihIgraca(string ProslijedjenaImena)
        {
            //prvo ocistiti regex
            var matches = Regex.Matches(ProslijedjenaImena, "(?:^|\\s)(?<username>[^\\s]*)(?=\\s|$)");// rezultati su u prvoj grupi
            List<Igrac> svePrijave = new List<Igrac>();
            foreach (Match i in matches)
            {
                string KorisnickoIme = i.Groups["username"].Value;
                Igrac noviIgrac = _listaIgraca.Where(x => x.PrikaznoIme == KorisnickoIme).FirstOrDefault();//korisnicka imena su unique
                svePrijave.Add(noviIgrac);
            }
            return svePrijave;
        }
        public bool TakmicenjaViseOd(string naziv, int ID, List<Takmicenje> listaTakmicenja)
        {
            if (listaTakmicenja.Where(s => s.Naziv == naziv && s.ID != ID).Count() > 0)
                return true;
            return false;
        }
        public List<(string key, string error)> VratiListuErroraAkcijaEdit(EditTakmicenjeVM objekat,
            List<Takmicenje> ListaTakmicenja)
        {
            List<(string key, string error)> listaErrora = new List<(string key, string error)>();

            if (TakmicenjaViseOd(objekat.Naziv, objekat.ID, ListaTakmicenja))
                listaErrora.Add((nameof(objekat.Naziv), "Vec postoji takmicenje u bazi"));
            if (objekat.RokZavrsetkaPrijave != null && objekat.RokPocetkaPrijave != null &&
                objekat.RokZavrsetkaPrijave < objekat.RokPocetkaPrijave)
                listaErrora.Add((nameof(objekat.RokZavrsetkaPrijave), "Datum zavrsetka prijava ne moze biti prije pocetka"));
            if (objekat.DatumPocetka != null && objekat.RokZavrsetkaPrijave != null && objekat.DatumPocetka < objekat.RokZavrsetkaPrijave)
                listaErrora.Add((nameof(objekat.DatumPocetka), "Datum pocetka ne moze biti prije zavrsetka prijava"));
            if (objekat.DatumPocetka != null && objekat.DatumZavrsetka != null && objekat.DatumZavrsetka < objekat.DatumPocetka)
                listaErrora.Add((nameof(objekat.DatumZavrsetka), "Datum pocetka takmicenja ne moze biti prije zavrsetka"));

            return listaErrora;
        }
    }
}
