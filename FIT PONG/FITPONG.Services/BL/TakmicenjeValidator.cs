using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FIT_PONG.Database.DTOs;
using FIT_PONG.SharedModels.Requests.Takmicenja;

namespace FIT_PONG.Services.BL
{
    public class TakmicenjeValidator
    {
        public List<string> _listaTakmicenja { get; set; } = new List<string>();
        public List<Igrac> _listaIgraca { get; set; } = new List<Igrac>();

        public List<(string key, string error)> VratiListuErroraAkcijaDodaj(TakmicenjaInsert objekat,
            List<string> ListaTakmicenja, List<Igrac> ListaIgraca)
        {
            _listaTakmicenja = ListaTakmicenja;
            _listaIgraca = ListaIgraca;

            List<(string key, string error)> listaErrora = new List<(string key, string error)>();
            if (PostojiTakmicenje(objekat.Naziv))
                listaErrora.Add(("", "Već postoji takmičenje u bazi"));
            if (!objekat.RucniOdabir)
            {
                if (objekat.RokZavrsetkaPrijave != null && objekat.RokZavrsetkaPrijave != null &&
                  objekat.RokZavrsetkaPrijave < objekat.RokPocetkaPrijave)
                    listaErrora.Add((nameof(objekat.RokZavrsetkaPrijave), "Datum završetka prijava ne može biti prije početka"));
                if (objekat.DatumPocetka != null && objekat.RokZavrsetkaPrijave != null && objekat.DatumPocetka < objekat.RokZavrsetkaPrijave)
                    listaErrora.Add((nameof(objekat.DatumPocetka), "Datum početka ne moze biti prije završetka prijava"));
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
                        listaErrora.Add(("", "Molimo unesite ispravno imena igrača"));
                    }
                    if (RucnaImenaSadrziDuplikate(objekat.RucnoOdabraniIgraci))
                        listaErrora.Add(("", "Igrače navodite samo jednom"));
                    if (BrojRucnoUnesenih(objekat.RucnoOdabraniIgraci) < 4)
                        listaErrora.Add(("", "Minimalno 4 igrača za takmičenje"));
                }
                else
                {
                    listaErrora.Add(("", "Molimo unesite ispravno imena igrača"));
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
            int brojUkupnoMatcheva = match.Count;
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
                if (x.Groups["username"].Success && !String.IsNullOrEmpty(x.Groups["username"].Value) 
                    && !String.IsNullOrWhiteSpace(x.Groups["username"].Value))
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
                if (i.Groups["username"].Success && !String.IsNullOrEmpty(i.Groups["username"].Value)
                   && !String.IsNullOrWhiteSpace(i.Groups["username"].Value))
                {
                    string KorisnickoIme = i.Groups["username"].Value;
                    Igrac noviIgrac = _listaIgraca.Where(x => x.PrikaznoIme == KorisnickoIme).FirstOrDefault();//korisnicka imena su unique
                    svePrijave.Add(noviIgrac);
                }
            }
            return svePrijave;
        }
        public bool TakmicenjaViseOd(string naziv, int ID, List<Takmicenje> listaTakmicenja)
        {
            if (listaTakmicenja.Where(s => s.Naziv == naziv && s.ID != ID).Count() > 0)
                return true;
            return false;
        }
        public List<(string key, string error)> VratiListuErroraAkcijaEdit(TakmicenjaUpdate objekat,int ID,
            List<Takmicenje> ListaTakmicenja, Takmicenje bazaObj)
        {
            List<(string key, string error)> listaErrora = new List<(string key, string error)>();
                                                //OVDJE CE BITI PROSLIJEDJEN ID PARAMETAR OVOJ FUNKCIJI
            if (TakmicenjaViseOd(objekat.Naziv, ID, ListaTakmicenja))
                listaErrora.Add((nameof(objekat.Naziv), "Već postoji takmičenje u bazi"));
            if (bazaObj.Inicirano)
            {
                if (objekat.RokZavrsetkaPrijave != null && objekat.RokPocetkaPrijave != null &&
                    objekat.RokZavrsetkaPrijave < objekat.RokPocetkaPrijave)
                    listaErrora.Add((nameof(objekat.RokZavrsetkaPrijave), "Datum završetka prijava ne moze biti prije početka takmičenja"));
                if (objekat.DatumPocetka != null && objekat.RokZavrsetkaPrijave != null && objekat.DatumPocetka < objekat.RokZavrsetkaPrijave)
                    listaErrora.Add((nameof(objekat.DatumPocetka), "Datum početka ne može biti prije završetka prijava"));
            }
            else
                listaErrora.Add(("", "Takmičenje je već inicirano, nije moguće mijenjati datume prijava"));
            if (objekat.DatumPocetka != null && objekat.DatumZavrsetka != null && objekat.DatumZavrsetka < objekat.DatumPocetka)
                listaErrora.Add((nameof(objekat.DatumZavrsetka), "Datum početka takmičenja ne može biti prije završetka"));

            return listaErrora;
        }
    }
}
