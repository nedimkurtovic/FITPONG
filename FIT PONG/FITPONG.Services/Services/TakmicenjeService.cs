using AutoMapper;
using FIT_PONG.Database;
using FIT_PONG.Database.DTOs;
using FIT_PONG.Services.BL;
using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests.Takmicenja;
using MailKit;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace FIT_PONG.Services.Services
{
    public class TakmicenjeService:ITakmicenjeService
    {
        private readonly MyDb db;
        private readonly Evidentor evidentor;
        private readonly InitTakmicenja initTakmicenja;
        private readonly TakmicenjeValidator validator;
        private readonly IMapper mapko;

        public TakmicenjeService(MyDb _db, Evidentor _evidentor, InitTakmicenja _initTakmicenja,
            TakmicenjeValidator _validator, IMapper _mapko)
        {
            db = _db;
            evidentor = _evidentor;
            initTakmicenja = _initTakmicenja;
            validator = _validator;
            mapko = _mapko;
        }
        #region RealDeal
        public List<Takmicenja> Get(TakmicenjeSearch obj)
        {
            var qry = db.Takmicenja.AsQueryable();
            if (!string.IsNullOrWhiteSpace(obj.Naziv))
                qry = qry.Where(x => x.Naziv.Contains(obj.Naziv));
            
            var TakmicenjaPovratni = qry
                .Include(x=>x.Sistem).Include(x=>x.Status)
                .Include(x=>x.Kategorija).Include(x=>x.Vrsta)
                .Include(x=>x.Prijave)
                .Where(x=>x.Kategorija.Opis == obj.Kategorija || obj.Kategorija == null)
                .Where(x => x.Vrsta.Naziv == obj.Vrsta || obj.Vrsta == null)
                .Where(x => x.Sistem.Opis == obj.Sistem || obj.Sistem == null)
                .Where(x=>x.MinimalniELO == obj.MinimalniELO || obj.MinimalniELO == null)
                .OrderByDescending(x=>x.ID).ToList();
            var povratnaLista = new List<SharedModels.Takmicenja>();
            foreach(var i in TakmicenjaPovratni)
            {
                SharedModels.Takmicenja tak = mapko.Map<SharedModels.Takmicenja>(i);
                tak.Sistem = i.Sistem.Opis;
                tak.Kategorija = i.Kategorija.Opis;
                tak.Vrsta = i.Vrsta.Naziv;
                tak.Status = i.Status.Opis;
                tak.Prijave = i.Prijave;
                povratnaLista.Add(tak);
            }
            return povratnaLista;
        }

        public Takmicenja GetByID(int id)
        {
            var obj = db.Takmicenja
                .Include(x => x.Sistem)
                .Include(x => x.Status)
                .Include(x => x.Kategorija)
                .Include(x => x.Prijave)
                .Include(x => x.Vrsta).Where(x => x.ID == id).FirstOrDefault();
            if(obj == null)
            {
                throw new UserException("Takmicenje ne postoji");
            }
            SharedModels.Takmicenja tak = mapko.Map<SharedModels.Takmicenja>(obj);
            tak.Sistem = obj.Sistem.Opis;
            tak.Kategorija = obj.Kategorija.Opis;
            tak.Vrsta = obj.Vrsta.Naziv;
            tak.Status = obj.Status.Opis;
            tak.Prijave = obj.Prijave;
            return tak;
        }

        public Takmicenja Add(TakmicenjaInsert objekat, int KreatorID)
        {
            
            ValidirajAddTakmicenja(objekat);
            var novo = mapko.Map<Takmicenje>(objekat);
            using (var transakcija = db.Database.BeginTransaction())//sigurnost u opasnim situacijama 
            {
                try
                {
                    novo.KreatorID = KreatorID;
                    Feed TakmicenjeFeed = new Feed
                    {
                        Naziv = novo.Naziv + " feed",
                        DatumModifikacije = DateTime.Now
                    };
                    db.Feeds.Add(TakmicenjeFeed);
                    db.SaveChanges();
                    novo.FeedID = TakmicenjeFeed.ID;

                    var statusKreiran = db.StatusiTakmicenja.Where(x => x.Opis == "Kreirano").FirstOrDefault();
                    novo.StatusID = statusKreiran.ID;

                    db.Add(novo);
                    db.SaveChanges();

                    //dobaviti igrace iz regexa
                    if (objekat.RucniOdabir)
                    {
                        validator._listaIgraca = db.Igraci.ToList();
                        List<Igrac> svi = validator.GetListaRucnihIgraca(objekat.RucnoOdabraniIgraci);
                        foreach (Igrac i in svi)
                        {
                            Prijava novaPrijava = new Prijava
                            {
                                DatumPrijave = DateTime.Now,
                                isTim = false,
                                Naziv = i.PrikaznoIme,
                                TakmicenjeID = novo.ID
                            };

                            novaPrijava.StanjePrijave = new Stanje_Prijave(novaPrijava.ID);
                            db.Prijave.Add(novaPrijava);
                            db.SaveChanges();

                            Prijava_igrac PrijavaIgracPodatak = new Prijava_igrac
                            {
                                IgracID = i.ID,
                                PrijavaID = novaPrijava.ID
                            };
                            db.PrijaveIgraci.Add(PrijavaIgracPodatak);
                            db.SaveChanges();
                        }
                    }
                    transakcija.Commit();
                    var povratni = GetByID(novo.ID); // zbog includeova i to lakse odozgo nego da ponavljam kod ovdje
                    return povratni;
                }
                catch (Exception ex)
                {
                    transakcija.Rollback();
                    throw new UserException("Greška prilikom spašavanja u bazu");
                }
            }
        }
       
        public Takmicenja Update(int id, TakmicenjaUpdate obj)
        {
            var objBaza = db.Takmicenja.Where(x => x.ID == id).FirstOrDefault();
            if (objBaza == null)// neka ovog dijela ovdje, jer kad ga premjestim u validirajUpdatetakmicenja, onda 
                //cu dole pomjeriti ovu prethodnu i naredne 3 linije koda, i onda cu opet morat ako prodje ta funkcija
                //opet ovdje dobavljati objekat baze
            {
                throw new UserException("Takmičenje ne postoji");
            }
            //ako nije inicirano mozes mijenjati odredjene atribute, dodati korisniku mogucnost
            //mijenjanja/ dodavanja / uklanjanja novih/starih igraca?
            ValidirajUpdateTakmicenja(obj, id, objBaza);
            //mapko.Map(obj, objBaza);//dje si sta ima
            //dakle definitivno, ako se zeli nesto updateovati, ne smije se poslati null inace ce se
            //to tretira kao da niste zeljeli to updateovati, bio bi problem da postoji neki atribut
            //koji se moze nulirati nekim slucajem, sreca ovdje to i nije bas slucaj, tj jest za 
            //ove datume ali ako si poslao null a onaj je vec nuliran onda se tretira kao null nikom nista
            //onda ces moci promijeniti vrijednost u neku konkretnu kad iduci put posaljes request,
            //however neces moc nulirat nazad jebiga
            if (!String.IsNullOrWhiteSpace(obj.Naziv))
                objBaza.Naziv = obj.Naziv;
            objBaza.DatumPocetka = obj.DatumPocetka ?? objBaza.DatumPocetka;
            objBaza.DatumZavrsetka = obj.DatumZavrsetka ?? objBaza.DatumZavrsetka;
            objBaza.VrstaID = obj.VrstaID ?? objBaza.VrstaID;
            objBaza.StatusID = obj.StatusID ?? objBaza.StatusID;
            objBaza.KategorijaID = obj.KategorijaID ?? objBaza.KategorijaID;
            objBaza.MinimalniELO = obj.MinimalniELO ?? objBaza.MinimalniELO;
            objBaza.RokPocetkaPrijave = obj.RokPocetkaPrijave ?? objBaza.RokPocetkaPrijave;
            objBaza.RokZavrsetkaPrijave = obj.RokZavrsetkaPrijave ?? objBaza.RokZavrsetkaPrijave;

            db.SaveChanges();
            var povratni = GetByID(objBaza.ID); // zbog includeova i to lakse odozgo nego da ponavljam kod ovdje
            return povratni;
        }

        public Takmicenja Delete(int id)
        {
            // dobro razmisliti da li ce se ovo implementirati, skupa je malo operacija, ili mozda ograniciti samo
            // da se moze obrisati ako nije takmicenje inicirano, tj neko koristio rucni unos ofo ono pa skonto 
            //da je zajebo pa treba ponovo(vidis treba skontat dugme za kreatora, dodaj jos igraca ili nesto)
            Takmicenje obj = db.Takmicenja
                .Include(x => x.Feed).Include(x => x.Prijave)
                .Where(c => c.ID == id).SingleOrDefault();
            if (obj == null)
            {
                throw new UserException("Takmicenje ne postoji");
            }
            ValidirajDeleteTakmicenja(obj);
            var temp = mapko.Map<Takmicenja>(obj);
            using (var transakcija = db.Database.BeginTransaction())
            {
                try
                {
                    List<FeedObjava> feedObjavas = db.FeedsObjave.Where(x => x.FeedID == obj.FeedID).ToList();
                    foreach (FeedObjava fo in feedObjavas)
                    {
                        List<Objava> objave = db.Objave.Where(x => x.ID == fo.ObjavaID).ToList();
                        db.Objave.RemoveRange(objave);
                        db.FeedsObjave.Remove(fo);
                    }

                    db.Feeds.Remove(obj.Feed);

                    foreach (Prijava i in obj.Prijave)
                    {
                        List<Prijava_igrac> lista = db.PrijaveIgraci.Where(x => x.PrijavaID == i.ID).ToList();
                        List<Stanje_Prijave> stanjaprijave = db.StanjaPrijave.Where(x => x.PrijavaID == i.ID).ToList();
                        db.RemoveRange(lista);
                        db.RemoveRange(stanjaprijave);
                        db.Prijave.Remove(i);
                    }
                    db.Takmicenja.Remove(obj);
                    db.SaveChanges();
                    transakcija.Commit();
                    return temp;

                }
                catch (DbUpdateException)
                {
                    transakcija.Rollback();
                    throw new UserException("Greška prilikom spašavanja u bazu");
                }
            }

        }

        public Takmicenja Initialize(int id)
        {
            var obj = db.Takmicenja
                .Include(tak => tak.Kategorija)
                .Include(tak => tak.Sistem)
                .Include(tak => tak.Vrsta)
                .Include(tak => tak.Status)
                .Include(tak => tak.Feed)
                .Include(tak => tak.Bracketi)
                .Include(tak => tak.Prijave).SingleOrDefault(y => y.ID == id);
            if (obj == null)
            {
                throw new UserException("Takmicenje ne postoji");
            }
            ValidirajInitTakmicenja(obj);
            try
            {
                initTakmicenja.GenerisiRaspored(obj);
            }
            catch (Exception) // mislim da samo moze biti dbupdate exception ali hajd neka exception
            {
                throw new Exception("Došlo je do greške prilikom inicijalizovanja takmičenja");
            }
            var povratni = GetByID(id); // zbog includeova i to lakse odozgo nego da ponavljam kod ovdje
            return povratni;
        }

        public List<EvidencijaMeca> GetEvidencije(string KorisnikUsername, int takmid)
        {
            Igrac igrac = evidentor.NadjiIgraca(KorisnikUsername);
            List<Utakmica> NjegoveUtakmice = evidentor.DobaviUtakmice(igrac, takmid);
            List<EvidencijaMeca> model = new List<EvidencijaMeca>();
            foreach (Utakmica i in NjegoveUtakmice)
            {
                //ne bi se smjelo nikada desiti da se nadje null igracID jer je na frontendu prikazano samo ono gdje su oba igraca unesena..
                //to je rjeseno onom funkcijom JelBye unutar funkcije DobaviUtakmice u par linija koda iznad
                EvidencijaMeca nova = new EvidencijaMeca();
                List<Igrac_Utakmica> svaUcesca = db.IgraciUtakmice.Where(x => x.UtakmicaID == i.ID).ToList();
                List<(Prijava pr, Igrac_Utakmica ucesce)> Timovi = new List<(Prijava pr, Igrac_Utakmica ucesce)>();
                foreach (Igrac_Utakmica j in svaUcesca)
                {
                    Prijava prijavaJoinUcesce = evidentor.GetPrijavuZaUcesce(j, takmid);
                    Timovi.Add((prijavaJoinUcesce, j));
                }
                (List<Igrac_Utakmica> Tim1, List<Igrac_Utakmica> Tim2) TimoviFinalni = evidentor.VratiUcescaPoTimu(Timovi);
                //dovoljno je provjeriti samo za jednog igraca, a svakako radi i za varijantu kad je double jer oba igraca pripadaju istoj prijavi koja ima //isti naziv
                string NazivTim1 = Timovi.Where(x => x.ucesce == TimoviFinalni.Tim1[0]).Select(x => x.pr.Naziv).FirstOrDefault();
                string NazivTim2 = Timovi.Where(x => x.ucesce == TimoviFinalni.Tim2[0]).Select(x => x.pr.Naziv).FirstOrDefault();

                nova.Tim1 = TimoviFinalni.Tim1;
                nova.Tim2 = TimoviFinalni.Tim2;

                nova.NazivTim1 = NazivTim1;
                nova.NazivTim2 = NazivTim2;

                nova.RezultatTim1 = null;
                nova.RezultatTim1 = null;

                //ovo ne bi trebalo smetati webappu obzirom da nece ovaj atribut nigdje koristit a webapiju je bitno
                //zbog ponovnog dobavljanja liste igraca
                nova.UtakmicaID = TimoviFinalni.Tim1[0].UtakmicaID;
                model.Add(nova);
            }
            return model;   
        }

        public EvidencijaMeca EvidentirajMec(int takmid, EvidencijaMeca obj)
            //ovdje realno se moze vratiti trenutno evidentirani mec, ili pozvati metodu iznad GetEvidencije
            //i onda mu samo vrati ovo sto mu preostalo za evidentirati
        {
            ValidirajEvidencijuMeca(obj);
            //nikad ne bi niti jedan tim trebao biti null da napomenem, to je rijeseno u evidencijimeca httpget    
            if(!evidentor.EvidentirajMec(obj, takmid))
                throw new UserException("Greška prilikom spašavanja zapisa");
            return obj;
        }
      
        public List<RasporedStavka> GetRaspored(int id)
        {
            Takmicenje obj = db.Takmicenja.Where(x => x.ID == id).FirstOrDefault();
            if (obj == null)
            {
                throw new UserException("Takmičenje ne postoji ili je obrisano");
            }
            List<RasporedStavka> parovi = new List<RasporedStavka>();
            List<Utakmica> sveNaTakmicenju = db.Utakmice.AsNoTracking()
                .Include(x => x.UcescaNaUtakmici)
                .Include(x => x.Runda).ThenInclude(x => x.Bracket).ThenInclude(x => x.Takmicenje)
                .Where(x => x.Runda.Bracket.TakmicenjeID == id).ToList();

            foreach (Utakmica i in sveNaTakmicenju)
            {
                (string tim1, int? rez1, int? rez2, string tim2) par = evidentor.GetPar(i, id);
                RasporedStavka nova = new RasporedStavka { 
                    Tim1 = par.tim1,
                    Tim2 = par.tim2,
                    RezultatTim1 = par.rez1,
                    RezultatTim2 = par.rez2,
                    Runda = i.Runda.BrojRunde
                };
                parovi.Add(nova);
            }
            
            return parovi;
        }
       
        public List<TabelaStavka> GetTabela(int id)
        {
            Takmicenje obj = db.Takmicenja.Where(x => x.ID == id).FirstOrDefault();
            if (obj == null)
            {
                throw new UserException("Takmičenje ne postoji ili je obrisano");
            }
            var parovi = new List<TabelaStavka>();
            List<Utakmica> sveNaTakmicenju = db.Utakmice.AsNoTracking()
         .Include(x => x.UcescaNaUtakmici)
         .Include(x => x.Runda).ThenInclude(x => x.Bracket).ThenInclude(x => x.Takmicenje)
         .Where(x => x.Runda.Bracket.TakmicenjeID == id).ToList();

            
            foreach (Utakmica i in sveNaTakmicenju)
            {
                (string tim1, int? rez1, int? rez2, string tim2) par = evidentor.GetPar(i, id);
                UbaciUTabelu(par, ref parovi);
            }
            parovi = parovi.OrderByDescending(x => x.Pobjeda).ToList();
            return parovi;
        }
        public EvidencijaMeca GetIgraceZaEvidenciju(EvidencijaMeca obj, int takmid)
        {
            var rezultat = evidentor.DobaviIgraceZaEvidencijuMeca(obj, takmid);
            if (rezultat == null)
                throw new UserException("Neispravni podaci poslani");
            return rezultat;
        }

        public List<Users> GetBlokiraneIgrace(int takmId)
        {
            var takmicenje = db.Takmicenja.Find(takmId);

            if (takmicenje == null)
                throw new UserException("Takmicenje ne postoji u bazi.");

            var blokLista = db.BlokListe.Where(b => b.TakmicenjeID == takmId).Include(b=>b.Igrac).Select(b=>b.Igrac).ToList();

            return mapko.Map<List<SharedModels.Users>>(blokLista);
        }

        public Prijave BlokirajPrijavu(int takmId, int prijavaId)
        {

            Prijava p = db.Prijave.Include(d => d.Takmicenje).Where(x => x.ID == prijavaId && x.TakmicenjeID==takmId).SingleOrDefault();

            if (p == null)
                throw new UserException("Ne postoji prijava u bazi.");

            Stanje_Prijave sp = db.StanjaPrijave.Where(x => x.PrijavaID == prijavaId).SingleOrDefault();
            if (sp != null)
                db.Remove(sp);
            
            List<Prijava_igrac> pi = db.PrijaveIgraci.Where(x => x.PrijavaID == prijavaId).ToList();

            if (pi != null)
            {
                BlokLista blok1 = new BlokLista
                {
                    IgracID = pi[0].IgracID,
                    TakmicenjeID = p.TakmicenjeID
                };

                db.Add(blok1);

                if (pi != null && pi.Count > 1)
                {
                    BlokLista blok2 = new BlokLista
                    {
                        IgracID = pi[1].IgracID,
                        TakmicenjeID = p.TakmicenjeID
                    };

                    db.Add(blok2);
                    db.Remove(pi[1]);
                }

                db.Remove(pi[0]);
                db.Remove(p);
                db.SaveChanges();

                return mapko.Map<Prijave>(p);
            }
            throw new UserException("Ne postoji zapis u tabeli Prijava_Igrac.");
        }
        #endregion

        #region Pomagaci
        private void UbaciUTabelu((string tim1, int? rez1, int? rez2, string tim2) par
            , ref List<TabelaStavka> parovi)
        {
            if (par.tim1 != null)
            {
                if (!parovi.Select(x => x.Naziv).Contains(par.tim1))
                    parovi.Add(new TabelaStavka { Naziv = par.tim1, Pobjeda = 0, Poraza = 0, UkupnoOdigrano = 0 });
                if (par.rez1 != null && par.rez2 != null)
                {
                    bool pobjeda = (par.rez1 > par.rez2);
                    if (pobjeda)
                        parovi.Where(x => x.Naziv == par.tim1).FirstOrDefault().Pobjeda++;
                    else
                        parovi.Where(x => x.Naziv == par.tim1).FirstOrDefault().Poraza++;
                    parovi.Where(x => x.Naziv == par.tim1).FirstOrDefault().UkupnoOdigrano++;
                }
            }
            if (par.tim2 != null)
            {
                if (!parovi.Select(x => x.Naziv).Contains(par.tim2))
                    parovi.Add(new TabelaStavka { Naziv = par.tim2, Pobjeda = 0, Poraza = 0, UkupnoOdigrano = 0 });
                if (par.rez1 != null && par.rez2 != null)
                {
                    bool pobjeda = (par.rez1 < par.rez2);
                    if (pobjeda)
                        parovi.Where(x => x.Naziv == par.tim2).FirstOrDefault().Pobjeda++;
                    else
                        parovi.Where(x => x.Naziv == par.tim2).FirstOrDefault().Poraza++;
                    parovi.Where(x => x.Naziv == par.tim2).FirstOrDefault().UkupnoOdigrano++;
                }
            }

        }
        #endregion
        //=============================ZONA VALIDACIJE ISPOD, NASTAVITI S PAŽNJOM !==============================
        #region Validacija
        private bool ValidirajAddTakmicenja(TakmicenjaInsert obj)
        {
            //var igraci = db.Igraci.ToList();
            //var listaTakmicenja = db.Takmicenja.Select(x => x.Naziv).ToList();
            ////ako ce biti prbolema onda ce biti kod provera gdje su hardkodirane 
            ////vrijednosti(unutar ovog ove funkcije u validatoru), vrstaID 

            var listaErrora = validator.VratiListuErroraAkcijaDodaj(obj);
            RegulisiListuErrora(listaErrora);
            return true;
        }
        private bool ValidirajInitTakmicenja(Takmicenje obj)
        {
            var listaErrora = initTakmicenja.VratiListuErrora(obj);
            RegulisiListuErrora(listaErrora);
            return true;

        }
        private bool ValidirajUpdateTakmicenja(TakmicenjaUpdate obj, int _takmicenjeid, Takmicenje objBaza)
        {
            var listaErrora = validator.VratiListuErroraAkcijaEdit(obj, _takmicenjeid,objBaza);
            RegulisiListuErrora(listaErrora);
            return true;
        }
        private bool ValidirajDeleteTakmicenja(Takmicenje bazaObj)
        {
            var listaErrora = new List<(string key, string msg)>();
            if (bazaObj.Inicirano)
                listaErrora.Add((nameof(bazaObj.Inicirano), "Takmičenje se može obrisati samo ako nije inicirano"));
            RegulisiListuErrora(listaErrora);
            return true;
        }
        private bool ValidirajEvidencijuMeca(EvidencijaMeca obj)
        {
            List<string> errori = evidentor.VratiListuErrora(obj);
            var PrevedenaLista = new List<(string key, string msg)>();
            foreach (string i in errori)
                PrevedenaLista.Add(("", i));
            RegulisiListuErrora(PrevedenaLista);
            return true;
        }
        //ideja za ime -> private bool CistaListaErrora, to bi genijalno bilo, raja ce poludit kad vidi
        private void RegulisiListuErrora(List<(string key, string msg)> listaErrora)
        {
            if (listaErrora.Count() > 0)
            {
                UserException ex = new UserException();
                foreach ((string key, string msg) in listaErrora)
                    ex.AddError(key, msg);
                throw ex;
            }
        }

       
        #endregion
    }
}
