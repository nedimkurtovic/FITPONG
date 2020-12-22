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
        private readonly ELOCalculator eloCalculator;

        public TakmicenjeService(MyDb _db, Evidentor _evidentor, InitTakmicenja _initTakmicenja,
            TakmicenjeValidator _validator, IMapper _mapko, ELOCalculator _EloCalculator)
        {
            db = _db;
            evidentor = _evidentor;
            initTakmicenja = _initTakmicenja;
            validator = _validator;
            mapko = _mapko;
            eloCalculator = _EloCalculator;
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
                catch (Exception)
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

        public SharedModels.Favoriti GetFavoriti(int id, int userId)
        {
            var obj = GetFavoritiPomocna(id, userId);

            return obj;
        }

        public SharedModels.Favoriti OznaciUtakmicu(int id, int userId)
        {
            var fav = db.Favoriti.Where(d => d.UserID == userId && d.UtakmicaId == id).SingleOrDefault();
            var u = db.Utakmice.Include(d => d.Runda).ThenInclude(d => d.Bracket).Where(d => d.ID == id).SingleOrDefault();
            if (u != null)
            {
                if (fav != null)
                {
                    db.Remove(fav);
                    db.SaveChanges();
                }
                else
                {
                    var novi = new Database.DTOs.Favoriti
                    {
                        UserID = userId,
                        UtakmicaId = id
                    };

                    db.Add(novi);
                    db.SaveChanges();
                }

                var obj = GetFavoritiPomocna(u.Runda.Bracket.TakmicenjeID, userId);
                return obj;
            }
            throw new UserException("Doslo je do greske prilikom spremanja favorita.");
        }

        public List<string> GetListaUseraNotifikacije(int utakId)
        {
            var favoriti = db.Favoriti.Include(d => d.User).Where(d => d.UtakmicaId == utakId).ToList();

            var lista = new List<string>();

            foreach (var item in favoriti)
            {
                lista.Add(item.User.Email);
            }
            return lista;
        }

        public List<(Prijave prijava, double vjerovatnoca)> PredictWinners(int takmId)
        {
            var takmicenje = db.Takmicenja
                .Include(x => x.Sistem)
                .Where(x=>x.ID == takmId).FirstOrDefault();

            if (takmicenje == null)
                throw new UserException("Takmicenje ne postoji u bazi.");

            ValidirajPredictWinners(takmicenje);

            List<(Prijava prijava, int Elo)> PrijaveLista = GetPrijaveSaElom(takmId);
            double [,] MatricaVjerovatnoca  = new double[PrijaveLista.Count,PrijaveLista.Count];
            for(int i = 0; i < PrijaveLista.Count-1; i++)
            {
                for (int j = i+1; j < PrijaveLista.Count; j++)
                {
                    double vjerovatnocaA = eloCalculator.GetVjerovatnoca(PrijaveLista[i].Elo, PrijaveLista[j].Elo);
                    double vjerovatnocaB = eloCalculator.GetVjerovatnoca(PrijaveLista[j].Elo, PrijaveLista[i].Elo);
                    MatricaVjerovatnoca[i,j] = vjerovatnocaA;
                    MatricaVjerovatnoca[j,i] = vjerovatnocaB;
                }
            }

            int brojRundi = initTakmicenja.pomocnaFunkcijaIzracunajRunde(takmicenje.Sistem, PrijaveLista.Count).BrojRundi;
            //RED SE ODNOSI NA SLOTNUM - 1 DAKLE NIJE PO RANKU SORTIRANO NEGO KAKO UTAKMICE IDU
            double[,] VjerovatnocePobjedeRunde = new double[PrijaveLista.Count, brojRundi];
            List<(Prijava pr, double vjerovatnoca)> finalnaLista = new List<(Prijava pr, double vjerovatnoca)>();
            for (int r = 0; r < brojRundi; r++)
            {
                for (int i = 0; i < PrijaveLista.Count; i++)
                {
                    double vjerovatnocaProslu = GetProsluVjerovatnocu( i, r - 1, VjerovatnocePobjedeRunde);
                    int v = GetSlotNumber(i+1, r+1) - 1;
                    int u = v + (int)Math.Round(Math.Pow(2, r)) - 1;
                    double suma = 0;
                    for (int k = v; k <= u; k++)
                    {
                        suma += (MatricaVjerovatnoca[i,k] *
                            GetProsluVjerovatnocu(k, r-1 ,VjerovatnocePobjedeRunde));
                    }
                    suma *= vjerovatnocaProslu;
                    VjerovatnocePobjedeRunde[i, r] = suma;
                    if(r == brojRundi - 1)
                        finalnaLista.Add((PrijaveLista[i].prijava, suma));               
                }
            }
            finalnaLista = finalnaLista.OrderByDescending(x => x.vjerovatnoca).ToList();
            var povratna = new List<(Prijave prijava, double vjerovatnoca)>();
            for (int i = 0; i < 3; i++)
            {
                if (i >= finalnaLista.Count)
                    break;
                var prijavaMap = mapko.Map<Prijave>(finalnaLista[i].pr);
                povratna.Add((prijavaMap, finalnaLista[i].vjerovatnoca));
            }
            return povratna;
        }

        public bool IsVlasnik(int takmicenjeId, string username)
        {
            var takmObj = db.Takmicenja.Where(x => x.ID == takmicenjeId).FirstOrDefault();
            if (takmObj == null)
                throw new UserException("Takmičenje ne postoji");
            var korisnik = db.Users.Where(x => x.UserName == username).FirstOrDefault();
            return takmObj.KreatorID == korisnik.Id;
        }

        #endregion

        #region Pomagaci
        private int GetSlotNumber(int igrac, int runda)
        {
            double prvi = Math.Round(Math.Pow(2, runda - 1));
            double drugi = Math.Round(Math.Pow(2, runda + 1));
            double treci = Math.Round(Math.Pow(2, runda));

            int vrijednost = 1 + (int)prvi +
                 (int)Math.Round(drugi * Math.Floor((double)(igrac - 1) / treci)) -
                 (int)Math.Round(prvi * Math.Floor((double)(igrac - 1) / prvi));
            
            return vrijednost;
        }
        private double GetProsluVjerovatnocu(int i, int runda, double[,] _vjerovatnoce)
        {
            if (runda < 0)
                return 1;
            return _vjerovatnoce[i,runda];
        }
        private List<(Prijava ,int)> GetPrijaveSaElom(int takmId)
        {
            var lista = new List<(Prijava, int)>();
   
            var utakmiceNaTakm = db.Utakmice
                .Include(x => x.Runda).ThenInclude(x => x.Bracket).ThenInclude(x => x.Takmicenje)
                .Include(x => x.UcescaNaUtakmici)
                .Where(x=>x.Runda.Bracket.TakmicenjeID==takmId && x.Runda.BrojRunde == 1)
                .OrderBy(x=>x.BrojUtakmice)
                .ToList();

            List<Prijava> listaPrijava = db.Takmicenja.Include(x => x.Prijave)
                .Where(x => x.ID == takmId).Select(x => x.Prijave).FirstOrDefault();
            
            foreach (var i in utakmiceNaTakm)
            {
                bool dodanNull = false;
                foreach (var j in i.UcescaNaUtakmici)
                {
                    Prijava prijava = null;
                    if (j.IgracID != null)
                        prijava = evidentor.GetPrijavuZaUcesce(j, takmId, listaPrijava);
                    bool pronadjena = false;
                    for (int c = 0; c < lista.Count; c++)
                    {
                        if (dodanNull && lista[c].Item1 == null && prijava == null)
                        {
                            pronadjena = true;
                            break;
                        }
                        else if (lista[c].Item1 != null && prijava != null && lista[c].Item1.ID == prijava.ID)
                        {
                            var novaVrijednost = (lista[c].Item2 + j.PristupniElo.GetValueOrDefault()) / 2;
                            lista[c] = (prijava, novaVrijednost);
                            pronadjena = true;
                            break;
                        }
                    }
                    if (!pronadjena)
                    {
                        lista.Add((prijava, j.PristupniElo.GetValueOrDefault(0)));
                        if (prijava == null)
                            dodanNull = true;
                    }

                }
            }
            return lista;
        }
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

        private SharedModels.Favoriti GetFavoritiPomocna(int id, int userId)
        {
            List<Utakmica> utakmice = db.Utakmice.AsNoTracking()
                .Include(x => x.UcescaNaUtakmici)
                .Include(x => x.Runda).ThenInclude(x => x.Bracket).ThenInclude(x => x.Takmicenje)
                .Where(x => x.Runda.Bracket.TakmicenjeID == id && !x.IsEvidentirana).ToList();

            SharedModels.Favoriti obj = new SharedModels.Favoriti();

            foreach (var item in utakmice)
            {
                var fav = db.Favoriti.Where(i => i.UserID == userId && i.UtakmicaId == item.ID).SingleOrDefault();

                (string tim1, int? rez1, int? rez2, string tim2) par = evidentor.GetPar(item, id);
                if (fav != null)
                    obj.oznaceneUtakmice.Add((par.tim1, par.rez1, par.rez2, par.tim2, item.ID));
                else
                    obj.neoznaceneUtakmice.Add((par.tim1, par.rez1, par.rez2, par.tim2, item.ID));
            }

            return obj;
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
        private bool ValidirajPredictWinners(Takmicenje bazaObj)
        {
            var listaErrora = new List<(string key, string msg)>();
            if (!bazaObj.Inicirano)
                listaErrora.Add((nameof(bazaObj.Inicirano), "Predikcija je moguća samo ako je generisan raspored"));
            if(bazaObj.Sistem.Opis != "Single elimination")
                listaErrora.Add((nameof(bazaObj.Sistem), "Trenutno je samo za Single elimination implementirana predikcija"));
            
            RegulisiListuErrora(listaErrora);
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
