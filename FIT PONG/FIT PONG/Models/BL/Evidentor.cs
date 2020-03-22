using FIT_PONG.ViewModels.TakmicenjeVMs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Models.BL
{
    public class Evidentor
    {
        private readonly MyDb db;
        private readonly ELOCalculator ELOCalculator;
        public InitTakmicenja inicijalizator { get; set; }
        public Evidentor(MyDb instanca, ELOCalculator _ELOCalculator)
        {
            db = instanca;
            ELOCalculator = _ELOCalculator;
        }

        public List<string> VratiListuErrora(EvidencijaMecaVM obj)
        {
            List<string> errori = new List<string>();
            if (obj.RezultatTim1.GetValueOrDefault() == obj.RezultatTim2.GetValueOrDefault())
            {
                errori.Add("Rezultati ne mogu biti jednaki");
            }
            if (!ValidanRezultat(obj.RezultatTim1.GetValueOrDefault()) || !ValidanRezultat(obj.RezultatTim2.GetValueOrDefault()))
            {
                errori.Add("Unesite pravilan broj osvojenih setova");
            }
            if (VecEvidentiranaUtakmica(obj.Tim1))
            {
                errori.Add("Utakmica je vec evidentirana!");
            }
            return errori;
        }
        public bool EvidentirajMec(EvidencijaMecaVM obj)
        {
            //nikad ne bi niti jedan tim trebao biti null da napomenem, to je rijeseno u evidencijimeca httpget
            using (var transakcija = db.Database.BeginTransaction())
            {
                try
                {

                    int pobjednik = (obj.RezultatTim1.GetValueOrDefault() > obj.RezultatTim2.GetValueOrDefault()) ? 1 : 2;
                    UpdateIgracUtakmicaZapis(obj, pobjednik);
                    //potrebno mec postaviti na zavrsen
                    Utakmica trenutna = db.IgraciUtakmice
                            .AsNoTracking().Include(x => x.Utakmica).ThenInclude(x => x.Runda)
                            .Include(x => x.Utakmica.UcescaNaUtakmici)
                            .Where(x => x.IgID == obj.Tim1[0].IgID)
                            .Select(x => x.Utakmica).FirstOrDefault();

                    if (GetTakmicenjeSistem(obj.TakmicenjeID) == "Single elimination"
                        && !ZadnjaUtakmica(obj.Tim1[0], obj.TakmicenjeID))
                    {

                        if (pobjednik == 1)
                            UnaprijediIgraceNaUtakmicu(obj.Tim1, trenutna);
                        else
                            UnaprijediIgraceNaUtakmicu(obj.Tim2, trenutna);
                    }
                    db.Utakmice.Where(x => x.ID == trenutna.ID).FirstOrDefault().IsEvidentirana = true;

                    db.SaveChanges();

                    if (ZadnjaUtakmica(obj.Tim1[0], obj.TakmicenjeID))
                    {
                        //prebaci takmicenje na zavrseno il whatever
                        Status_Takmicenja zavrseno = db.StatusiTakmicenja.Where(x => x.Opis == "Zavrseno" || x.Opis == "Završeno").FirstOrDefault();
                        db.Takmicenja.Where(x => x.ID == obj.TakmicenjeID).FirstOrDefault().StatusID = zavrseno.ID;
                        db.SaveChanges();
                    }
                    transakcija.Commit();
                    return true;
                }
                catch (Exception err)
                {
                    transakcija.Rollback();
                }
            }
            return false;

        }
        public string GetTakmicenjeSistem(int takmicenjeID)
        {
            string sistem = db.Takmicenja.AsNoTracking()
                .Include(x => x.Sistem)
                .Where(x => x.ID == takmicenjeID).Select(x => x.Sistem.Opis).FirstOrDefault();
            return sistem;
        }
        //pobjednik je predstavljen intom, ako je prvi tim pobjednik onda je int = 1 ako je drugi onda je int = 2
        public void UpdateIgracUtakmicaZapis(EvidencijaMecaVM obj, int pobjednik)
        {
            bool parovi = (obj.Tim1.Count() == 2) ? true : false;
            if (!parovi)
                UpdateIgracUtakmicaSingle(obj, pobjednik);
            else
                UpdateIgracUtakmicaDouble(obj, pobjednik);
            return;
        }
        public void UpdateIgracUtakmicaSingle(EvidencijaMecaVM obj, int pobjednik)
        {
            Tip_Rezultata pobjeda = db.TipoviRezultata.AsNoTracking().Where(x => x.Naziv == "Pobjeda").FirstOrDefault();
            Tip_Rezultata poraz = db.TipoviRezultata.AsNoTracking().Where(x => x.Naziv == "Poraz").FirstOrDefault();
            Igrac_Utakmica x = db.IgraciUtakmice
                .AsNoTracking()
                .Where(x => x.IgID == obj.Tim1[0].IgID).FirstOrDefault();

            x.OsvojeniSetovi = obj.RezultatTim1.GetValueOrDefault();
            int xs = (pobjednik == 1) ? 1 : 0; // predstavlja score x - naziv ucesca, slovo s - score

            Igrac_Utakmica y = db.IgraciUtakmice
                .AsNoTracking()
                .Where(x => x.IgID == obj.Tim2[0].IgID).FirstOrDefault();
            y.OsvojeniSetovi = obj.RezultatTim2.GetValueOrDefault();
            int ys = (pobjednik == 2) ? 1 : 0; // predstavlja score y - naziv ucesca, slovo s - score


            Igrac xigrac = db.Igraci.AsNoTracking().Where(c => c.ID == x.IgracID).FirstOrDefault();
            Igrac yigrac = db.Igraci.AsNoTracking().Where(c => c.ID == y.IgracID).FirstOrDefault();
            x.PristupniElo = xigrac.ELO;
            y.PristupniElo = yigrac.ELO;

            string X = "pobjeda";
            string Y = "poraz";

            if (xs == 1)
            {
                x.TipRezultata = pobjeda;
                y.TipRezultata = poraz;
            }
            else
            {
                x.TipRezultata = poraz;
                y.TipRezultata = pobjeda;
                X = "poraz";
                Y = "pobjeda";
            }
            UpdateStatistikuIStanjePrijave(x.IgracID ?? default(int), X=="pobjeda" ? true : false, false);
            UpdateStatistikuIStanjePrijave(y.IgracID ?? default(int), Y=="pobjeda" ? true : false, false);

            db.Update(x);
            db.Update(y);
            db.SaveChanges();

            xigrac.ELO = ELOCalculator.VratiEloSingle(x.PristupniElo.GetValueOrDefault(), y.PristupniElo.GetValueOrDefault(), xs);
            yigrac.ELO = ELOCalculator.VratiEloSingle(y.PristupniElo.GetValueOrDefault(), x.PristupniElo.GetValueOrDefault(), ys);

            db.Igraci.Where(x => x.ID == xigrac.ID).FirstOrDefault().ELO = xigrac.ELO;
            db.SaveChanges();
            db.Igraci.Where(x => x.ID == yigrac.ID).FirstOrDefault().ELO = yigrac.ELO;
            db.SaveChanges();

            return;
        }

        public void UpdateIgracUtakmicaDouble(EvidencijaMecaVM obj, int pobjednik)
        {
            Tip_Rezultata pobjeda = db.TipoviRezultata.AsNoTracking().Where(x => x.Naziv == "Pobjeda").FirstOrDefault();
            Tip_Rezultata poraz = db.TipoviRezultata.AsNoTracking().Where(x => x.Naziv == "Poraz").FirstOrDefault();
            Igrac_Utakmica x1 = db.IgraciUtakmice.AsNoTracking()
                .Where(x => x.IgID == obj.Tim1[0].IgID).FirstOrDefault();
            Igrac_Utakmica x2 = db.IgraciUtakmice.AsNoTracking()
                .Where(x => x.IgID == obj.Tim1[1].IgID).FirstOrDefault();

            x1.OsvojeniSetovi = obj.RezultatTim1.GetValueOrDefault();
            x2.OsvojeniSetovi = obj.RezultatTim1.GetValueOrDefault();
            int xs = (pobjednik == 1) ? 1 : 0; // predstavlja score x - naziv ucesca, slovo s - score

            Igrac_Utakmica y1 = db.IgraciUtakmice.AsNoTracking()
                .Where(x => x.IgID == obj.Tim2[0].IgID).FirstOrDefault();
            Igrac_Utakmica y2 = db.IgraciUtakmice.AsNoTracking()
                .Where(x => x.IgID == obj.Tim2[1].IgID).FirstOrDefault();

            y1.OsvojeniSetovi = obj.RezultatTim2.GetValueOrDefault();
            y2.OsvojeniSetovi = obj.RezultatTim2.GetValueOrDefault();
            int ys = (pobjednik == 2) ? 1 : 0; // predstavlja score x - naziv ucesca, slovo s - score


            Igrac x1igrac = db.Igraci.AsNoTracking().Where(c => c.ID == x1.IgracID).FirstOrDefault();
            Igrac x2igrac = db.Igraci.AsNoTracking().Where(c => c.ID == x2.IgracID).FirstOrDefault();
            Igrac y1igrac = db.Igraci.AsNoTracking().Where(c => c.ID == y1.IgracID).FirstOrDefault();
            Igrac y2igrac = db.Igraci.AsNoTracking().Where(c => c.ID == y2.IgracID).FirstOrDefault();

            x1.PristupniElo = x1igrac.ELO;
            x2.PristupniElo = x2igrac.ELO;

            y1.PristupniElo = y1igrac.ELO;
            y2.PristupniElo = y2igrac.ELO;

            string X = "pobjeda";
            string Y = "poraz";
            if (xs == 1)
            {
                x1.TipRezultata = pobjeda;
                x2.TipRezultata = pobjeda;
                y1.TipRezultata = poraz;
                y2.TipRezultata = poraz;
            }
            else
            {
                x1.TipRezultata = poraz;
                x2.TipRezultata = poraz;
                y1.TipRezultata = pobjeda;
                y2.TipRezultata = pobjeda;
                X = "poraz";
                Y = "pobjeda";
            }
            UpdateStatistikuIStanjePrijave(x1.IgracID ?? default(int), X=="pobjeda" ? true : false, true);
            UpdateStatistikuIStanjePrijave(x2.IgracID ?? default(int), X=="pobjeda" ? true : false, true);
            UpdateStatistikuIStanjePrijave(y1.IgracID ?? default(int), Y=="pobjeda" ? true : false, true);
            UpdateStatistikuIStanjePrijave(y2.IgracID ?? default(int), Y=="pobjeda" ? true : false, true);

            db.IgraciUtakmice.Update(x1);
            db.IgraciUtakmice.Update(x2);
            db.IgraciUtakmice.Update(y1);
            db.IgraciUtakmice.Update(y2);
            db.SaveChanges();

            x1igrac.ELO = ELOCalculator.VratiEloDouble(
                x1.PristupniElo.GetValueOrDefault(),
                x2.PristupniElo.GetValueOrDefault(),
                y1.PristupniElo.GetValueOrDefault(),
                y2.PristupniElo.GetValueOrDefault(), xs);

            x2igrac.ELO = ELOCalculator.VratiEloDouble(
                x2.PristupniElo.GetValueOrDefault(),
                x1.PristupniElo.GetValueOrDefault(),
                y1.PristupniElo.GetValueOrDefault(),
                y2.PristupniElo.GetValueOrDefault(), xs);

            y1igrac.ELO = ELOCalculator.VratiEloDouble(
                y1.PristupniElo.GetValueOrDefault(),
                y2.PristupniElo.GetValueOrDefault(),
                x1.PristupniElo.GetValueOrDefault(),
                x2.PristupniElo.GetValueOrDefault(), ys);

            y2igrac.ELO = ELOCalculator.VratiEloDouble(
                y2.PristupniElo.GetValueOrDefault(),
                y1.PristupniElo.GetValueOrDefault(),
                x1.PristupniElo.GetValueOrDefault(),
                x2.PristupniElo.GetValueOrDefault(), ys);

            db.Igraci.Where(x => x.ID == x1igrac.ID).FirstOrDefault().ELO = x1igrac.ELO;
            db.Igraci.Where(x => x.ID == x2igrac.ID).FirstOrDefault().ELO = x2igrac.ELO;
            db.Igraci.Where(x => x.ID == y1igrac.ID).FirstOrDefault().ELO = y1igrac.ELO;
            db.Igraci.Where(x => x.ID == y2igrac.ID).FirstOrDefault().ELO = y2igrac.ELO;
            db.SaveChanges();


        }
        public bool ZadnjaUtakmica(Igrac_Utakmica ucesce, int takmid)
        {
            int brojUtakmica = BrojUtakmicaNaTakmicenju(takmid);
            Utakmica x = db.IgraciUtakmice.AsNoTracking().Include(x => x.Utakmica).Where(x => x.IgID == ucesce.IgID)
                .Select(x => x.Utakmica)
                .FirstOrDefault();
            return x.BrojUtakmice == brojUtakmica;
        }
        public int BrojUtakmicaNaTakmicenju(int takmId)
        {
            Takmicenje tak = db.Takmicenja.AsNoTracking()
                .Include(x => x.Bracketi).ThenInclude(x => x.Runde).ThenInclude(x => x.Utakmice)
                .Where(x => x.ID == takmId)
                .FirstOrDefault();
            // za sad je samo jedan bracket ako se u buducnosti bude implementirao sistem sa vise bracketa morat ce se ovdje promijeniti
            //da budu 2 foreachpetlje 
            int sumaUtakmica = 0;
            foreach (Runda i in tak.Bracketi[0].Runde)
            {
                sumaUtakmica += i.Utakmice.Count();
            }
            return sumaUtakmica;

        }
        public bool VecEvidentiranaUtakmica(List<Igrac_Utakmica> ucesce)
        {
            return db.IgraciUtakmice.AsNoTracking().Include(x => x.Utakmica).Where(x => x.IgID == ucesce[0].IgID)
                .Select(x => x.Utakmica.IsEvidentirana).FirstOrDefault();
        }
        public bool ValidanRezultat(int rezultat)
        {
            return rezultat >= 0 && rezultat <= 5;
        }
        public (List<Igrac_Utakmica> Tim1, List<Igrac_Utakmica> Tim2) VratiUcescaPoTimu(List<(Prijava pr, Igrac_Utakmica ucesce)> Timovi)
        {
            List<Igrac_Utakmica> Tim1 = new List<Igrac_Utakmica>();
            List<Igrac_Utakmica> Tim2 = new List<Igrac_Utakmica>();
            Tim1.Add(Timovi[0].ucesce);
            foreach ((Prijava pr, Igrac_Utakmica ucesce) i in Timovi.Skip(1))
            {
                if (i.pr.ID == Timovi[0].pr.ID)
                    Tim1.Add(i.ucesce);
                else
                    Tim2.Add(i.ucesce);
            }
            return (Tim1, Tim2);


        }
        public Prijava GetPrijavuZaUcesce(Igrac_Utakmica ucesce, int takmID)
        {
            List<Prijava> listaPrijava = db.Takmicenja.Include(x => x.Prijave).Where(x => x.ID == takmID).Select(x => x.Prijave).FirstOrDefault();
            foreach (Prijava i in listaPrijava)
            {
                List<Prijava_igrac> prijavljeniIgraci = db.PrijaveIgraci.Where(x => x.PrijavaID == i.ID).ToList();
                //ovdje ima blaga mana, ako igrac kojim slucajem ucestvuje/igra za vise od jednog tima, onda nikad nece ova funkcija vratiti druge
                //timove, vec samo onaj prvi to je jedan od problema koji nismo rijesili, nisam siguran kako legalno stoje pravila sto se tice toga
                if (PrijavaSadrziIgraca(ucesce.IgracID, prijavljeniIgraci))
                    return i;
            }
            return null;

        }
        public bool PrijavaSadrziIgraca(int? igracID, List<Prijava_igrac> listaIgraca)
        {
            if (igracID == null)
                return false;
            foreach (Prijava_igrac i in listaIgraca)
            {
                if (i.IgracID == igracID.GetValueOrDefault())
                    return true;
            }
            return false;
        }
        public List<Utakmica> DobaviUtakmice(Igrac igrac, int takmid)
        {
            List<Utakmica> Pocetne = db.Utakmice
                            .Include(x => x.Runda).ThenInclude(x => x.Bracket).ThenInclude(x => x.Takmicenje)
                            .Include(x => x.UcescaNaUtakmici)
                            .Where(x => x.Runda.Bracket.Takmicenje.ID == takmid && x.IsEvidentirana == false).ToList();
            List<Utakmica> Filtrirane = new List<Utakmica>();
            foreach (Utakmica i in Pocetne)
            {
                foreach (Igrac_Utakmica j in i.UcescaNaUtakmici)
                {
                    if (j.IgracID.GetValueOrDefault() == igrac.ID)
                    {
                        //Ova funkcija JelBye je ponovo iskoristena kako se ne bi duplicirao kod, sada je svejedno da li je u prvoj rundi ili nije,
                        //ovdje nas samo interesuje da li je suparnik null, jer ako jeste to znaci da je igrac tek prosao u tu 
                        //utakmicu(ili se tek generisao raspored a on dobio bye) pa takvu utakmicu necemo evidentirati dok se ne
                        //postave svi igraci na tu utakmicu cime cemo zadovoljiti zahtjev da se mogu evidentirati samo mecevi na 
                        //kojima su rasporedjeni igraci
                        List<Igrac_Utakmica> tempLista = new List<Igrac_Utakmica>();
                        //nekako trebam doci da ubacim i kolegu ako su doubleovi u ovu temp listu
                        //vrati ucesca za prijavu
                        Prijava p = GetPrijavuZaUcesce(j, takmid);
                        tempLista = VratiUcescaNaUtakmiciZaPrijavu(p, i, takmid);
                        if (!JelBye(tempLista, i, false))//konkretno ovim pozivom provjeravam da li su svi igraci postavljeni na utakmici
                        {
                            if (!SadrziUtakmicu(Filtrirane, i))
                                Filtrirane.Add(i);
                            break;
                        }
                    }
                }
            }
            return Filtrirane;
        }
        public bool SadrziUtakmicu(List<Utakmica> sve, Utakmica trazena)
        {
            foreach (Utakmica i in sve)
            {
                if (i.ID == trazena.ID)
                    return true;
            }
            return false;
        }
        public List<Igrac_Utakmica> VratiUcescaNaUtakmiciZaPrijavu(Prijava p, Utakmica u, int takmid)
        {
            List<Igrac_Utakmica> povratna = new List<Igrac_Utakmica>();
            foreach (Igrac_Utakmica i in u.UcescaNaUtakmici)
            {
                Prijava temp = GetPrijavuZaUcesce(i, takmid);
                if (temp != null && temp.ID == p.ID)
                    povratna.Add(i);
            }
            return povratna;
        }
        public Igrac NadjiIgraca(string username)
        {
            int id = db.Users.Where(x => x.UserName == username).Select(x => x.Id).FirstOrDefault();
            Igrac ig = db.Igraci.Where(x => x.ID == id).FirstOrDefault();
            return ig;
        }

        private void UpdateStatistikuIStanjePrijave(int id, bool pobjeda, bool isTim)
        {
            Statistika s = db.Statistike.Where(d => d.IgracID == id && d.AkademskaGodina == DateTime.Now.Year).SingleOrDefault();
            Prijava_igrac p = db.PrijaveIgraci.Where(d => d.IgracID == id).FirstOrDefault();
            Stanje_Prijave sp = db.StanjaPrijave.Where(d => d.PrijavaID == p.PrijavaID).FirstOrDefault();
            
            if (s == null)
            {
                s = new Statistika(id);
                sp = new Stanje_Prijave(p.PrijavaID);
                db.Add(s);
                db.Add(sp);
                db.SaveChanges();
            }

            if (isTim)
            {
                if (pobjeda)
                {
                    s.BrojTimskihPobjeda++;
                    sp.BrojPobjeda++;
                    sp.BrojBodova += 2;
                }
                else
                    sp.BrojIzgubljenih++;
            }
            else
            {
                if (pobjeda)
                {
                    s.BrojSinglePobjeda++;
                    sp.BrojBodova += 2;
                    sp.BrojPobjeda++;
                }
                else
                    sp.BrojIzgubljenih++;

            }
            s.BrojOdigranihMeceva++;
            sp.BrojOdigranihMeceva++;
            db.Update(s);
            db.SaveChanges();
        }

        //=============================ZA PREMJESTANJE IGRACA NA UTAKMICU=============================\\
        public void UnaprijediIgraceNaUtakmicu(List<Igrac_Utakmica> PobjednickaUcesca, Utakmica trenutnaUtakmica)
        {
            // dvije varijante mogu biti koje ce zavisiti od toga sta se proslijedi iz funckije evidencija meca, dakle mogu se proslijediti direktno igracUtakmica, ili //prijava pa da se na osnovu prijave i trenutne utakmice nadju IgracUtakmica odnosno lista ucesca, mada je jednostavnija varijanta i nadam se da cemo tu 
            //uraditi da se konkretno proslijede IgracUtakmica zapisi a ne prijave 
            if (!JelBye(PobjednickaUcesca, trenutnaUtakmica)) // za svaki slucaj
            {
                foreach (Igrac_Utakmica i in PobjednickaUcesca)
                {
                    PremjestiNaIducuUtakmicu(i.IgracID.GetValueOrDefault(), trenutnaUtakmica);
                    db.SaveChanges();
                }
            }
        }
        //Ova funkcija ce biti dizajnirana tako da se poziva za jednog igraca,
        public void PremjestiNaIducuUtakmicu(int IgracID, Utakmica trenutnaUtakmica)
        {
            //bracket mi treba da ne bi morao raditi join do takmicenja, najblizi iduci filter koji bi mi dao rundu iz istog takmicenja
            //je bracket pa da skratim jedan join viska
            Runda runda = db.Utakmice
                .Include(x => x.Runda).ThenInclude(x => x.Utakmice).Include(x => x.Runda.Bracket)
                .AsNoTracking()
                .Where(x => x.ID == trenutnaUtakmica.ID).Select(x => x.Runda).FirstOrDefault();
            int bracketid = db.Brackets.Where(x => x.ID == runda.BracketID).FirstOrDefault().ID;
            int BrojPrveUtakmiceIduceRunde = runda.Utakmice.Min(x => x.BrojUtakmice);
            int nadjiIducuUtakmicu = inicijalizator.NadjiOdgovarajucuIducuUtakmicu(trenutnaUtakmica.BrojUtakmice,
                runda.Utakmice.Count(), BrojPrveUtakmiceIduceRunde);
            Utakmica iducaUtakmica = db.Utakmice.AsNoTracking().Include(x => x.UcescaNaUtakmici).Include(x => x.Runda).ThenInclude(x => x.Bracket)
                                            .Where(x => x.Runda.Bracket.ID == bracketid && x.BrojUtakmice == nadjiIducuUtakmicu).FirstOrDefault();
            int pozicijaUcesca = iducaUtakmica.OdgovarajuceMjestoUcesca();
            Igrac_Utakmica iduceUcesce = iducaUtakmica.UcescaNaUtakmici[pozicijaUcesca];
            db.IgraciUtakmice.Where(x => x.IgID == iduceUcesce.IgID).FirstOrDefault().IgracID = IgracID;
            db.SaveChanges();

        }
        public bool JelBye(List<Igrac_Utakmica> PobjednickaUcesca, Utakmica trenutnaUtakmica, bool IzPremjestajaPozvanaFunkcija = true)
        {
            //mislim da je dovoljno pitati samo za prvo ucesce da li je igrac null nebitno da li je single ili double elimination, jer kako su trenutno stvari 
            //organizovane, ne bi se nikad trebalo desiti da postoji u double varijanti 3 zapisa i 1 null, jedino nekom mozda visom silom ako je igrac obrisao 
            //akaunt u medjuvremenu ili nesto
            // ovdje je upitno da li mi treba uslov ako je prva runda, teoretski poslije se moze desiti AHA :: MOGUCNOST EVIDENCIJE MECA SE NE SMIJE PRIKAZIVATI
            //DOK NISU SVI IGRACI RASPOREDJENI NA UTAKMICU OSIM U SLUCAJU PRVE RUNDE GDJE JE MOGUCE DA SE DESI BYE, U SVIM OSTALIM SLUCAJEVIMA NE DOPUSTITI 	//IGRACU EVIDENCIJU UTAKMICE AKO NIJE PROTIVNICKI TIM/PRIJAVA POSTAVLJENA
            //ovo jos uvijek nije dalo odgovor na pitanje da li je potrebno ovdje pitati da li je prva runda u pitanju...
            if (IzPremjestajaPozvanaFunkcija)
            {
                int brojrunde = db.Utakmice.Include(x => x.Runda).AsNoTracking()
                    .Where(x => x.ID == trenutnaUtakmica.ID).Select(x => x.Runda.BrojRunde).FirstOrDefault();
                if (brojrunde != 1)
                    return false;
            }
            // za svaki slucaj nek se nadje ako neko bude dosjetljiv pa u doda u kveri string neki drugi id a potrefi	
            //svoj nekim slucajem
            //ovo bez pobjednickih ucesca ima dvosmisleno znacenje u zavisnosti koja je funkcija pozvala ovu funkciju, u svakom
            //slucaju to su filtrirana ucesca
            List<Igrac_Utakmica> bezPobjednickihUcesca = VratiBezUcesca(PobjednickaUcesca, trenutnaUtakmica.UcescaNaUtakmici);
            if (bezPobjednickihUcesca != null) // ne bi trebalo biti null ikad
            {
                if (bezPobjednickihUcesca[0].IgracID == null)
                    return true;
            }

            return false;

        }
        List<Igrac_Utakmica> VratiBezUcesca(List<Igrac_Utakmica> ucescaKojaTrebaIzbaciti, List<Igrac_Utakmica> svaUcesca)
        {
            List<Igrac_Utakmica> povratnaUcesca = new List<Igrac_Utakmica>();
            foreach (Igrac_Utakmica i in svaUcesca)
            {
                //lafo dvije muhe jednim udarcem jer ne pravim posebno za single i double vec jedna rjesava oba slucaja
                foreach (Igrac_Utakmica j in ucescaKojaTrebaIzbaciti)
                {
                    if (i.IgID != j.IgID && !SadrziUcesce(ucescaKojaTrebaIzbaciti, i))
                    {
                        povratnaUcesca.Add(i);
                        break;
                    }
                }
            }
            return povratnaUcesca;
        }
        public bool SadrziUcesce(List<Igrac_Utakmica> lista, Igrac_Utakmica zapis)
        {
            foreach (Igrac_Utakmica i in lista)
            {
                if (i.IgID == zapis.IgID)
                    return true;
            }
            return false;
        }
        public (string tim1, int? rez1, int? rez2, string tim2) GetPar(Utakmica u, int takmid)
        {
            List<(string naziv, int? rez, int igId)> lista = new List<(string naziv, int? rez, int igID)>();
            List<(string naziv, int? rez)> povratna = new List<(string naziv, int? rez)>();
            bool doubleovi = (u.UcescaNaUtakmici.Count == 4) ? true : false;
            foreach (Igrac_Utakmica i in u.UcescaNaUtakmici)
            {
                Prijava p = GetPrijavuZaUcesce(i, takmid);

                if (p != null)
                {
                    if (!SadrziPrijavuZaParove(lista, (p.Naziv, i.OsvojeniSetovi, i.IgID), doubleovi))
                        lista.Add((p.Naziv, i.OsvojeniSetovi, i.IgID));
                    continue;
                }
                else
                    if (!SadrziPrijavuZaParove(lista, (null, i.OsvojeniSetovi, i.IgID), doubleovi))
                {
                    lista.Add((null, null, i.IgID));
                }
            }
            //ako je sve kako treba, ovo bi i za single i za double uvijek trebalo vratiti 2 zapisa, tim1 i tim2
            if (lista.Count != 0)
            {
                return (lista[0].naziv, lista[0].rez, lista[1].rez, lista[1].naziv);
            }
            //ne bi nikada teoretski trebala ova linija hitat
            return (null, null, null, null);
        }
        private bool SadrziPrijavuZaParove(List<(string naziv, int? rez, int igId)> lista, (string naziv, int? rez, int igId) zapis, bool doubleovi)
        {
            foreach ((string naziv, int? rez, int igId) i in lista)
            {
                //ovdje je situacija da pod prepostavkom da ce igID uvijek se kreirati redoslijedom tj da ce ucesca na jednoj utakmici biti 
                //poredana kao 1 2 3 4, onda ce ovo važiti, ako bude frke onda ovdje se treba vratiti jer konkretno trenutna mana ove funckije je
                //nemogucnost razaznavanja ko je u kojem timu ako su nullovi igracID, tj kad niko nije postavljen na utakmici,kako znati ko je koji 
                //tim..
                if (zapis.naziv == null && i.naziv == null)
                {
                    if (!doubleovi && lista.Count == 1)// kad su singlovi a niko nije rasporedjen na utakmici, donji uslov uvijek vrati true
                        //pa sam morao imati ovaj poseban slucaj
                        return false;
                    if (zapis.igId == i.igId + 1)
                        return true;
                    else
                        continue;
                }

                if (i.naziv == zapis.naziv && i.igId != zapis.igId)
                    return true;
            }
            return false;
        }

        public List<string> GetZadnjeUtakmice(int brojUtakmica = 5)
        {
            List<string> povratne = new List<string>();
            List<(int takmid, Utakmica utakmica)> x = new List<(int takmid, Utakmica utakmica)>();
            List<Utakmica> zadnje = db.Utakmice.AsNoTracking().Include(x => x.UcescaNaUtakmici).Include(x => x.Runda).ThenInclude(x => x.Bracket).ThenInclude(x => x.Takmicenje)
                .Where(x => x.IsEvidentirana).OrderByDescending(x => x.DatumVrijeme).Take(brojUtakmica).ToList();
            foreach (Utakmica i in zadnje)
            {
                (string tim1, int? rez1, int? rez2, string tim2) par = GetPar(i, i.Runda.Bracket.TakmicenjeID);
                if (par.rez1 > par.rez2)
                    povratne.Add(par.tim1 + " je pobijedio/la " + par.tim2 + " rezultatom -> " + par.rez1 + " : " + par.rez2);
                else
                    povratne.Add(par.tim1 + " je izgubio/la od " + par.tim2 + " rezultatom -> " + par.rez1 + " : " + par.rez2);

            }
            return povratne;
        }
    }
}
