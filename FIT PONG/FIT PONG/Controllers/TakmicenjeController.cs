using FIT_PONG.Hubs;
using FIT_PONG.Models;
using FIT_PONG.Models.BL;
using FIT_PONG.ViewModels;
using FIT_PONG.ViewModels.TakmicenjeVMs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace FIT_PONG.Controllers
{
    public class TakmicenjeController : Controller
    {

        private readonly MyDb db;
        private readonly InitTakmicenja inicijalizator;
        private readonly ELOCalculator ELOCalculator;
        private readonly Evidentor evidentor;
        private readonly IHubContext<NotifikacijeHub> notifikacijeHub;

        public TakmicenjeController(MyDb instanca, 
            InitTakmicenja instancaInita, 
            ELOCalculator ELOCalculator,
            Evidentor _evidentor,
            IHubContext<NotifikacijeHub> notifikacijeHub)
        {
            db = instanca;
            inicijalizator = instancaInita;
            this.ELOCalculator = ELOCalculator;
            evidentor = _evidentor;
            evidentor.inicijalizator = instancaInita;//ne znam koliko je ovo sigurno i ima smisla, pokusat cu, samo jednu funkciju koristim 
            //ako bude frke izbacit cu ga skroz djeni zeve
            this.notifikacijeHub = notifikacijeHub;
        }
        public IActionResult Index(int page = 1, string sortExpression= "-DatumKreiranja")
        {
            List<TakmicenjeVM> takmicenja = db.Takmicenja
                .Include(tak => tak.Kategorija)
                .Include(tak => tak.Sistem)
                .Include(tak => tak.Vrsta)
                .Include(tak => tak.Status)
                .Include(tak => tak.Feed)
                .Include(tak => tak.Prijave)
                .Select(s => new TakmicenjeVM
                  (s, 0)).ToList();
            foreach (TakmicenjeVM i in takmicenja)
                i.BrojPrijavljenih = db.Prijave.Where(s => s.TakmicenjeID == i.ID).Count();
            var qry = takmicenja.OrderByDescending(X=>X.ID).ToList();
            var takmicenja1 = PagingList.Create(qry, 10, page, sortExpression, "ID");

            return View(takmicenja1);
        }

        public IActionResult Detalji(int? id)
        {
            if (id == null)
            {
                return View("/Takmicenje/Neuspjeh");
            }
            //potreban query za broj rundi,u bracketima se nalazi takmicenjeID ,bar bi trebalo opotrebna migracija
            TakmicenjeVM obj = GetTakmicenjeVM(id);
            if (obj != null)
                return View(obj);

            return Redirect("/Takmicenje/Neuspjeh");
        }

        public IActionResult RezultatiSingleDouble(int? id)
        {
            Takmicenje obj = db.Takmicenja.Where(x => x.ID == id).SingleOrDefault();
            if (obj != null && obj.Inicirano)
            {
                ViewBag.id = id;
                return PartialView();
            }
            //ovdje treba partial view s porukom nije generisan raspored jer ovo poziva ajax 
            ViewBag.poruka = "Raspored nije generisan";
            return PartialView("Neuspjeh");
        }

        public IActionResult RezultatiRoundRobin(int? id)
        {
            TakmicenjeVM obj = GetTakmicenjeVM(id);
            if (obj != null && obj.Inicirano.GetValueOrDefault())
            {
                ViewBag.id = id;
                ViewBag.brojRundi = obj.Bracketi[0].Runde.Count();
                return PartialView();
            }
            //ovdje treba partial view ista prica ko i gore
            ViewBag.poruka = "Raspored nije generisan";
            return PartialView("Neuspjeh");
        }

        public IActionResult EvidentirajMec(int? id)
        {
            TakmicenjeVM obj = GetTakmicenjeVM(id);
            ViewBag.id = id;
            ViewBag.brojRundi = obj.Bracketi[0].Runde.Count();

            return View();
        }

        public IActionResult Evidencija(EvidencijaVM model)
        {
            Igrac_Utakmica rezultat1 = db.IgraciUtakmice
                                            .Include(d => d.Igrac)
                                            .Include(d=>d.Utakmica)
                                            .Where(d => d.IgID == model.IgracUtakmicaId1)
                                            .SingleOrDefault();
            Igrac_Utakmica rezultat2 = db.IgraciUtakmice
                                            .Include(d => d.Igrac)
                                            .Include(d => d.Utakmica)
                                            .Where(d => d.IgID == model.IgracUtakmicaId2)
                                            .SingleOrDefault();
            if (rezultat1 != null && rezultat2 != null)//provjera da su dobavljeni validni objekti
            {
                if (model.Rezultat1!= null && model.Rezultat2!=null 
                    && model.Rezultat1 >= 0 && model.Rezultat1 <= 5 
                    && model.Rezultat2 >= 0 && model.Rezultat2 <= 5)
                {

                    rezultat1.OsvojeniSetovi = model.Rezultat1;
                    rezultat1.PristupniElo = rezultat1.Igrac.ELO;
                    rezultat2.OsvojeniSetovi = model.Rezultat2;
                    rezultat2.PristupniElo = rezultat2.Igrac.ELO;
                    if (model.Rezultat1 > model.Rezultat2)//provjera ko je pobjednik
                    {
                        rezultat1.TipRezultataID = 1;
                        rezultat2.TipRezultataID = 2;
                        rezultat1.Igrac.ELO = ELOCalculator.VratiEloSingle(rezultat1.Igrac.ELO, rezultat2.Igrac.ELO, 1);
                        rezultat2.Igrac.ELO = ELOCalculator.VratiEloSingle(rezultat2.Igrac.ELO, rezultat1.Igrac.ELO, 0);
                    }
                    else
                    {
                        rezultat1.TipRezultataID = 2;
                        rezultat2.TipRezultataID = 1;
                        rezultat1.Igrac.ELO = ELOCalculator.VratiEloSingle(rezultat1.Igrac.ELO, rezultat2.Igrac.ELO, 0);
                        rezultat2.Igrac.ELO = ELOCalculator.VratiEloSingle(rezultat2.Igrac.ELO, rezultat1.Igrac.ELO, 1);
                    }
                    rezultat1.Utakmica.DatumVrijeme = DateTime.Now;
                    rezultat2.Utakmica.DatumVrijeme = DateTime.Now;
                    db.Update(rezultat1);
                    db.Update(rezultat2);
                    db.SaveChanges();
                }
            }
            
            return Redirect("/Takmicenje");
        }

        private void PreracunajElo(DateTime DatumVrijeme, int igracId, int noviElo)
        {
            List<Igrac_Utakmica> rezultati = db.IgraciUtakmice.Include(d=>d.Utakmica)
                                                              .Include(d=>d.Igrac)
                                                              .Where(d => d.IgracID == igracId && d.Utakmica.DatumVrijeme > DatumVrijeme)
                                                              .OrderBy(d=>d.Utakmica.DatumVrijeme)
                                                              .ToList();
            foreach (var item in rezultati)
            {
                if (item.OsvojeniSetovi != null)
                {
                    int score = item.TipRezultataID ?? default(int);
                    item.PristupniElo = noviElo;
                    item.Igrac.ELO = 
                        ELOCalculator.VratiEloSingle(item.PristupniElo ?? default(int), 
                                       db.IgraciUtakmice.Where(d => d.UtakmicaID == item.UtakmicaID && d.IgracID!=item.IgracID)
                                       .Single()
                                       .PristupniElo ?? default(int),
                                       score - 1);
                    noviElo = item.Igrac.ELO;
                    db.Update(item);
                    db.SaveChanges();
                }
            }
        }


        [HttpPost]
        public IActionResult Dodaj(CreateTakmicenjeVM objekat)
        {
            if (ModelState.IsValid)
            {
                TakmicenjeValidator validator = new TakmicenjeValidator();
                List<(string key, string error)> listaerrora = validator.VratiListuErroraAkcijaDodaj(objekat,
                    db.Takmicenja.Select(x => x.Naziv).ToList(),
                    db.Igraci.ToList());

                if (listaerrora.Count() == 0)
                {
                    using (var transakcija = db.Database.BeginTransaction())//sigurnost u opasnim situacijama 
                    {
                        try
                        {
                            Takmicenje novo = new Takmicenje(objekat);
                            var idUser = db.Users.Where(x => x.UserName == HttpContext.User.Identity.Name).FirstOrDefault();
                            novo.KreatorID = idUser.Id;
                            Feed TakmicenjeFeed = new Feed
                            {
                                Naziv = novo.Naziv + " feed",
                                DatumModifikacije = DateTime.Now
                            };
                            db.Feeds.Add(TakmicenjeFeed);
                            db.SaveChanges();
                            novo.FeedID = TakmicenjeFeed.ID;

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
                            return Redirect("/Takmicenje/Prikaz/" + novo.ID);
                        }
                        catch (DbUpdateException er)
                        {
                            transakcija.Rollback();
                            ModelState.AddModelError("", "Doslo je do greške prilikom spašavanja u bazu");
                        }
                    }
                }
                else
                {
                    //ako je validator vratio errore ovdje cemo ih pametno stavit u modelstate kako bi se prikazali na viewu
                    foreach ((string key, string err) i in listaerrora)
                    {
                        ModelState.AddModelError(i.key, i.err);
                    }
                }
            }
            LoadViewBag();
            return View(objekat);
        }
        
        public IActionResult Dodaj()
        {
            LoadViewBag();
            return View();
        }
        public void LoadViewBag()
        {
            ViewBag.kategorije = db.Kategorije.Select(s => new ComboBoxVM { ID = s.ID, Opis = s.Opis }).ToList();
            ViewBag.sistemi = db.SistemiTakmicenja.Select(s => new ComboBoxVM { ID = s.ID, Opis = s.Opis }).ToList();
            ViewBag.vrste = db.VrsteTakmicenja.Select(s => new ComboBoxVM { ID = s.ID, Opis = s.Naziv }).ToList();
            ViewBag.statusi = db.StatusiTakmicenja.Select(s => new ComboBoxVM { ID = s.ID, Opis = s.Opis }).ToList();
        }

        public IActionResult Prikaz(int? id)
        {
            if (id == null)
            {
                return View("/Takmicenje/Neuspjeh");
            }
            //potreban query za broj rundi,u bracketima se nalazi takmicenjeID ,bar bi trebalo opotrebna migracija
            TakmicenjeVM obj = GetTakmicenjeVM(id);
            if (obj != null)
                return View(obj);
            
            return Redirect("/Takmicenje/Neuspjeh");
        }

        public IActionResult Edit(int id)
        {
            Takmicenje obj = db.Takmicenja.Find(id);
            var idUser = db.Users.Where(x => x.UserName == HttpContext.User.Identity.Name).FirstOrDefault();
            if (obj.KreatorID != idUser.Id)
                return VratiNijeAutorizovan();
            if (obj != null)
            {
                EditTakmicenjeVM ob1 = new EditTakmicenjeVM(obj);
                LoadViewBag();
                return View(ob1);
            }
            return Redirect("/Takmicenje/Neuspjeh");
        }
        [HttpPost]
        public IActionResult Edit(EditTakmicenjeVM objekat)//dodatiiii kod.....za rucni unos
        {
            if (ModelState.IsValid)
            {
                TakmicenjeValidator validator = new TakmicenjeValidator();
                List<(string key, string error)> listaerrora = validator.VratiListuErroraAkcijaEdit(objekat, db.Takmicenja.ToList());
                if (listaerrora.Count() == 0)
                {
                    Takmicenje obj = db.Takmicenja.Find(objekat.ID);
                    if (obj != null)
                    {
                        using (var transakcija = db.Database.BeginTransaction())
                        {
                            try
                            {
                                obj.Naziv = objekat.Naziv;
                                obj.DatumPocetka = objekat.DatumPocetka;
                                obj.DatumZavrsetka = objekat.DatumZavrsetka;
                                if (objekat.RokPocetkaPrijave != null)
                                    //samo ako su registracije otvorene promijeni ove ovdje stvari jer se one ne postavljaju na rucnom unosu
                                {
                                    obj.RokPocetkaPrijave = objekat.RokPocetkaPrijave;
                                    obj.RokZavrsetkaPrijave = objekat.RokZavrsetkaPrijave;
                                    obj.MinimalniELO = objekat.MinimalniELO ?? 0;
                                    obj.KategorijaID = objekat.KategorijaID;
                                    obj.VrstaID = objekat.VrstaID;
                                }
                                obj.StatusID = objekat.StatusID;
                                db.Update(obj);
                                db.SaveChanges();

                                Feed FidObjekat = db.Feeds.Find(obj.FeedID);
                                FidObjekat.Naziv = obj.Naziv + " feed";
                                FidObjekat.DatumModifikacije = DateTime.Now;
                                db.Update(FidObjekat);
                                db.SaveChanges();

                                transakcija.Commit();
                                return Redirect("/Takmicenje/Prikaz/" + obj.ID);
                            }
                            catch (DbUpdateException er)
                            {
                                transakcija.Rollback();
                            }

                        }
                    }
                }
                else
                {
                    //ako je validator vratio errore ovdje cemo ih pametno stavit u modelstate kako bi se prikazali na viewu
                    foreach ((string key, string err) i in listaerrora)
                    {
                        ModelState.AddModelError(i.key, i.err);
                    }
                }
            }
            LoadViewBag();
            return View(objekat);
        }
        public IActionResult VratiNijeAutorizovan()
        {
            ViewBag.poruka = "Niste autorizovani za takvu radnju";
            return View("Neuspjeh");
        }
        public IActionResult Obrisi(int? id)
        {
            if (id == null)
            {
                return View("/Takmicenje/Neuspjeh");
            }
            else
            {
                Takmicenje obj = db.Takmicenja.Find(id);
                var idUser = db.Users.Where(x => x.UserName == HttpContext.User.Identity.Name).FirstOrDefault();
                if (obj.KreatorID != idUser.Id)
                    return VratiNijeAutorizovan();
                if (obj != null)
                {
                    TakmicenjeVM takmicenjeobj = new TakmicenjeVM
                    {
                        ID = obj.ID,
                        Naziv = obj.Naziv
                    };
                    return View(takmicenjeobj);
                }
            }
            return Redirect("/Takmicenje/Neuspjeh");
        }
        [HttpPost]
        public IActionResult PotvrdaBrisanja(int ID)
        {
            //mozda se okrenut na talicev princip tj da ne budu 2 zahtjeva na bazu nego samo alert izbacit 
            //preko js : da li ste sigurni u operaciju,mada nece se svaki dan brisati takmicenje ne vjerujem da ce veliki load
            //biti na bazi,ali u svakom slucaju najmanji problem je skratit jednu funkciju i ubacit alert,ovaj princip je
            //svakako medju prvim koji sam naucio,ne znaci da je ispravan
            try
            {
                Takmicenje obj = db.Takmicenja.Include(x => x.Feed).Where(c => c.ID == ID).SingleOrDefault();
                db.Feeds.Remove(obj.Feed);
                db.Takmicenja.Remove(obj);
                db.SaveChanges();
                return Redirect("/Takmicenje/Index");
            }
            catch (DbUpdateException err)
            {

            }
            return Redirect("/Takmicenje/Neuspjeh");
        }

        public bool PostojiTakmicenje(string naziv)
        {
            if (db.Takmicenja.Where(s => s.Naziv == naziv).Count() > 0)
                return true;
            return false;
        }
        public IActionResult Uspjeh()
        {
            return View();
        }
        public IActionResult Neuspjeh()
        {
            return View();
        }

        public IActionResult NemaRezultata()
        {
            return PartialView();
        }


        [HttpPost]
        public IActionResult Prijava(TakmicenjePrijavaVM prijava)
        {
            Takmicenje t = db.Takmicenja.Find(prijava.takmicenjeID);

            if (ModelState.IsValid && t.RokZavrsetkaPrijave >= DateTime.Now)
            {
                Prijava_igrac pi = db.PrijaveIgraci.Where(p => p.Prijava.TakmicenjeID == prijava.takmicenjeID && p.IgracID == prijava.Igrac1ID).SingleOrDefault();
                if (pi != null)
                    ModelState.AddModelError(nameof(prijava.Igrac1ID), "Igrač je već prijavljen na takmičenje.");

                if (prijava.Igrac1ID == null)
                    ModelState.AddModelError(nameof(prijava.Igrac1ID), "Polje igrač1 je obavezno.");
              
                if (prijava.isTim)
                {
                    Prijava_igrac pi2 = db.PrijaveIgraci.Where(p => p.Prijava.TakmicenjeID == prijava.takmicenjeID && p.IgracID == prijava.Igrac2ID).SingleOrDefault();
                    if (pi2 != null)
                        ModelState.AddModelError(nameof(prijava.Igrac2ID), "Igrač je već prijavljen na takmičenje.");
                    if (prijava.Naziv == null)
                        ModelState.AddModelError(nameof(prijava.Naziv), "Polje naziv je obavezno.");
                    if (prijava.Igrac2ID == null)
                        ModelState.AddModelError(nameof(prijava.Igrac2ID), "Polje igrač2 je obavezno.");
                    if (db.BlokListe.Where(x => x.IgracID == prijava.Igrac2ID && x.TakmicenjeID == prijava.takmicenjeID).SingleOrDefault() != null)
                        ModelState.AddModelError(nameof(prijava.Igrac2ID), "Ovaj igrač je blokiran na ovom takmičenju.");
                }

                if (prijava.Igrac1ID == prijava.Igrac2ID && prijava.Igrac2ID != null)
                    ModelState.AddModelError(nameof(prijava.Igrac2ID), "Ne možete dodati istog igrača kao saigrača.");

                if (db.BlokListe.Where(x => x.IgracID == prijava.Igrac1ID && x.TakmicenjeID == prijava.takmicenjeID).SingleOrDefault() != null)
                    ModelState.AddModelError(nameof(prijava.Igrac1ID), "Blokirani ste na ovom takmičenju.");

                if (ModelState.ErrorCount == 0)
                {
                    Prijava nova = new Prijava
                    {
                        DatumPrijave = DateTime.Now,
                        TakmicenjeID = prijava.takmicenjeID,
                        isTim = prijava.isTim,
                        Naziv = prijava.Naziv
                    };
                    nova.StanjePrijave = new Stanje_Prijave(nova.ID);
                    if (!prijava.isTim)
                        nova.Naziv = db.Igraci.Find(prijava.Igrac1ID).PrikaznoIme;

                    if (PostojiLiPrijava(nova.Naziv, prijava.takmicenjeID))
                    {
                        ModelState.AddModelError(nameof(prijava.Naziv), "Ime je zauzeto.");
                        LoadViewBagPrijava(prijava.takmicenjeID);
                        return View(prijava);
                    }
                    db.Takmicenja.Find(prijava.takmicenjeID).Prijave.Add(nova);
                    db.SaveChanges();
                    KreirajPrijavuIgrac(prijava, nova.ID);

                    return Redirect("/Takmicenje/UspjesnaPrijava");
                }
            }
            LoadViewBagPrijava(prijava.takmicenjeID);

            return View(prijava);
        }

        public IActionResult UspjesnaPrijava()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Prijava(int takmID)
        {
            Takmicenje takm = db.Takmicenja.Where(t => t.ID == takmID).Include(t => t.Vrsta).SingleOrDefault();
            if (takm == null)
                return View("Neuspjeh");
            TakmicenjePrijavaVM tp = new TakmicenjePrijavaVM
            {
                takmicenjeID = takmID,
                isTim = true
            };

            if (takm.Vrsta.Naziv == "Single")
                tp.isTim = false;

            ViewBag.igraci = db.Igraci.Where(i => i.ELO >= takm.MinimalniELO && i.User.Email!=User.Identity.Name).Select(i => new ComboBoxVM { ID = i.ID, Opis = i.PrikaznoIme }).ToList();
            ViewBag.igrac = db.Igraci.Include(i=>i.User).Where(i => i.ELO >= takm.MinimalniELO && i.User.Email==User.Identity.Name).Select(i => new ComboBoxVM { ID = i.ID, Opis = i.PrikaznoIme }).ToList();       
            return View(tp);
        }


        public IActionResult Otkazi(int prijavaID)
        {
            Prijava p = db.Prijave.Find(prijavaID);
            if (p != null)
            {
                Stanje_Prijave sp = db.StanjaPrijave.Where(x => x.PrijavaID == prijavaID).SingleOrDefault();
                if (sp != null)
                    db.Remove(sp);
                List<Prijava_igrac> pi = db.PrijaveIgraci.Where(x => x.PrijavaID == prijavaID).ToList();
                if (pi != null && pi.Count > 1)
                {
                    db.Remove(pi[1]);
                }
                db.Remove(pi[0]);

                db.Remove(p);
                db.SaveChanges();
                return View("OtkazivanjePrijave");
            }
            return View("Neuspjeh");
        }

        private void LoadViewBagPrijava(int id)
        {
            Takmicenje takm = db.Takmicenja.Find(id);
            ViewBag.igraci = db.Igraci.Where(i => i.ELO >= takm.MinimalniELO && i.User.Email != User.Identity.Name).Select(i => new ComboBoxVM { ID = i.ID, Opis = i.PrikaznoIme }).ToList();
            ViewBag.igrac = db.Igraci.Include(i => i.User).Where(i => i.ELO >= takm.MinimalniELO && i.User.Email == User.Identity.Name).Select(i => new ComboBoxVM { ID = i.ID, Opis = i.PrikaznoIme }).ToList();
        }

        private void KreirajPrijavuIgrac(TakmicenjePrijavaVM prijava, int id)
        {

            Prijava_igrac prijava_Igrac1 = new Prijava_igrac
            {
                IgracID = prijava.Igrac1ID ?? default(int),
                PrijavaID = id
            };

            db.Add(prijava_Igrac1);

            if (prijava.isTim)
            {
                Prijava_igrac prijava_Igrac2 = new Prijava_igrac
                {
                    IgracID = prijava.Igrac2ID ?? default(int),
                    PrijavaID = id
                };
                db.Add(prijava_Igrac2);
            }
            db.SaveChanges();
        }

        public IActionResult BlokirajPrijavu(int prijavaID, string nazivTima)
        {
            Prijava p = db.Prijave.Include(d => d.Takmicenje).Where(p => p.ID == prijavaID).SingleOrDefault();
            if (p != null)
            {
                var userId = db.Users.Where(d => d.Email == User.Identity.Name).FirstOrDefault();
                if (p.Takmicenje.KreatorID != userId.Id)
                    return VratiNijeAutorizovan();

                Stanje_Prijave sp = db.StanjaPrijave.Where(x => x.PrijavaID == prijavaID).SingleOrDefault();
                if (sp != null)
                    db.Remove(sp);
                List<Prijava_igrac> pi = db.PrijaveIgraci.Where(x => x.PrijavaID == prijavaID).ToList();
                Takmicenje t = db.Takmicenja.Find(p.TakmicenjeID);
                BlokLista blok1 = new BlokLista
                {
                    IgracID = pi[0].IgracID,
                    TakmicenjeID = t.ID
                };
                db.Add(blok1);
                if (pi != null && pi.Count > 1)
                {
                    BlokLista blok2 = new BlokLista
                    {
                        IgracID = pi[1].IgracID,
                        TakmicenjeID = t.ID
                    };
                    db.Add(blok2);
                    db.Remove(pi[1]);
                }

                db.Remove(pi[0]);
                db.Remove(p);
                db.SaveChanges();
            }

            ViewBag.takmID = p.TakmicenjeID;
            return View("BlokirajPrijavuUspjeh");
        }
        private bool PostojiLiPrijava(string naziv, int id)
        {
            Prijava prijava = db.Prijave.Where(p => p.TakmicenjeID == id && p.Naziv == naziv).SingleOrDefault();
            if (prijava != null)
                return true;
            return false;
        }
        public IActionResult Init(int ID)//mozda ce biti i task
        {
            List<string> errors = new List<string>();
            Takmicenje _takmicenje = db.Takmicenja
                .Include(tak => tak.Kategorija)
                .Include(tak => tak.Sistem)
                .Include(tak => tak.Vrsta)
                .Include(tak => tak.Status)
                .Include(tak => tak.Feed)
                .Include(tak => tak.Bracketi)
                .Include(tak => tak.Prijave).SingleOrDefault(y => y.ID == ID);
            var idUser = db.Users.Where(x => x.UserName == HttpContext.User.Identity.Name).FirstOrDefault();
            if (_takmicenje.KreatorID != idUser.Id)
                return VratiNijeAutorizovan();
            if (_takmicenje != null && !_takmicenje.Inicirano)
            {
                if (_takmicenje.RokPocetkaPrijave != null && _takmicenje.RokZavrsetkaPrijave > DateTime.Now)
                    errors.Add("Rok registracija mora zavrsiti prije generisanja rasporeda");
                if (_takmicenje.Prijave.Count() < 4)
                    errors.Add("Takmicenje mora imati barem 4 igraca, otvorite ponovo registracije");
                if (errors.Count() == 0)
                {
                    using (var transakcija = db.Database.BeginTransaction())
                    {
                        try
                        {
                            inicijalizator.GenerisiRaspored(_takmicenje);
                            transakcija.Commit();
                            return RedirectToAction("Prikaz",new { id=_takmicenje.ID});
                        }
                        catch (Exception err)
                        {
                            transakcija.Rollback();
                            return Redirect("/Takmicenje/Neuspjeh");
                        }
                    }
                }
            }
            errors.Add("Takmicenje ne postoji ili je vec inicirano");
            return View("Neuspjeh", errors);
        }
        public IActionResult PrikaziFeed(int ID)
        {
            Takmicenje takm = db.Takmicenja.AsNoTracking().Include(x => x.Feed).Where(x=>x.ID==ID).FirstOrDefault();
            if (takm != null)
                return RedirectToAction("Prikaz", "Feed", new { id = takm.FeedID });
            return PartialView("Neuspjeh");
        }
        public TakmicenjeVM GetTakmicenjeVM(int? id)
        {

            //potreban query za broj rundi,u bracketima se nalazi takmicenjeID ,bar bi trebalo opotrebna migracija
            //Takmicenje obj = db.Takmicenja.Include(tak => tak.Kategorija)
            //                              .Include(tak => tak.Sistem)
            //                              .Include(tak => tak.Vrsta)
            //                              .Include(tak => tak.Status)
            //                              .Include(tak => tak.Feed)
            //                              .Include(tak => tak.Prijave)
            //                              .SingleOrDefault(y => y.ID == id);
            Takmicenje obj = db.Takmicenja.Include(tak => tak.Kategorija).
                Include(tak => tak.Sistem)
                .Include(tak => tak.Vrsta)
                .Include(tak => tak.Status)
                .Include(tak => tak.Feed)
                .Include(tak => tak.Prijave)
                .Include(x => x.Bracketi)
                .ThenInclude(x => x.Runde)
                .ThenInclude(x => x.Utakmice)
                .ThenInclude(x => x.UcescaNaUtakmici)
                .ThenInclude(x => x.Igrac)
                .SingleOrDefault(y => y.ID == id);
            if (obj != null)
                return new TakmicenjeVM(obj);
            return null;
        }

        
        //=============================ZA SAMU EVIDENCIJU UTAKMICE=============================\\
        [HttpGet]
        public IActionResult EvidencijaMeca(int id)
        {
            Igrac igrac = evidentor.NadjiIgraca(HttpContext.User.Identity.Name);
            List<Utakmica> NjegoveUtakmice = evidentor.DobaviUtakmice(igrac, id);
            List<EvidencijaMecaVM> model = new List<EvidencijaMecaVM>();
            foreach (Utakmica i in NjegoveUtakmice)
            {
                //ne bi se smjelo nikada desiti da se nadje null igracID jer je na frontendu prikazano samo ono gdje su oba igraca unesena..
                //to je rjeseno onom funkcijom JelBye unutar funkcije DobaviUtakmice u par linija koda iznad
                EvidencijaMecaVM nova = new EvidencijaMecaVM();
                List<Igrac_Utakmica> svaUcesca = db.IgraciUtakmice.Where(x => x.UtakmicaID == i.ID).ToList();
                List<(Prijava pr, Igrac_Utakmica ucesce)> Timovi = new List<(Prijava pr, Igrac_Utakmica ucesce)>();
                foreach (Igrac_Utakmica j in svaUcesca)
                {
                    Prijava prijavaJoinUcesce = evidentor.GetPrijavuZaUcesce(j, id);
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
                model.Add(nova);
            }
            ViewBag.id = id;
            return PartialView(model);
        }
        [HttpPost]
        public IActionResult EvidencijaMeca(EvidencijaMecaVM obj)
        {
            if (ModelState.IsValid)
            {       
                Igrac podnositeljZahtjeva = evidentor.NadjiIgraca(HttpContext.User.Identity.Name);
                if(!obj.Tim1.Select(x=>x.IgracID).Contains(podnositeljZahtjeva.ID) 
                    && !obj.Tim2.Select(x => x.IgracID).Contains(podnositeljZahtjeva.ID))
                {
                    return VratiNijeAutorizovan();
                }
                List<string> errori = evidentor.VratiListuErrora(obj);
                if (errori.Count() == 0)
                {
                    //nikad ne bi niti jedan tim trebao biti null da napomenem, to je rijeseno u evidencijimeca httpget    

                        try
                        {
                        if (evidentor.EvidentirajMec(obj)) { 

                            notifikacijeHub.Clients.All.SendAsync("startaj", GetListaUseraNotifikacije(obj.Tim1[0].UtakmicaID), obj.NazivTim1, obj.NazivTim2, obj.TakmicenjeID);
                            return RedirectToAction("EvidencijaMeca", new { id = obj.TakmicenjeID });
                        }
                        else
                        {
                            ModelState.AddModelError("", "Došlo je do nepredviđene greške, pokusajte opet");
                        }
                        }
                        catch(Exception err)
                        {
                            //mislim da se ovaj blok nikad nece hittat obzirom da imam try catch u evidentoru ali eto
                            ModelState.AddModelError("", "Došlo je do greške");
                        }
                }
                else
                {
                    foreach(string err in errori)
                    {
                        ModelState.AddModelError("", err);
                    }
                }
            }
            //nisam siguran da li je pametnije vratiti sve ili samo jedan ali eto
            List<EvidencijaMecaVM> temp = new List<EvidencijaMecaVM>();
            temp.Add(obj);
            ViewBag.id = obj.TakmicenjeID;
            return PartialView(temp);
        }

        public IActionResult GetTabela(int id)
        {
            Takmicenje obj = db.Takmicenja.Where(x => x.ID == id).FirstOrDefault();
            if (obj == null)
                return PartialView("Neuspjeh");
            List<TabelaStavkaVM> parovi = new List<TabelaStavkaVM>();
            List<Utakmica> sveNaTakmicenju = db.Utakmice.AsNoTracking()
         .Include(x => x.UcescaNaUtakmici)
         .Include(x => x.Runda).ThenInclude(x => x.Bracket).ThenInclude(x => x.Takmicenje)
         .Where(x => x.Runda.Bracket.TakmicenjeID == id).ToList();

            //ako nije inicirano vratiti ce samo praznu listu jer nema utakmica sto sam docekao kako treba tamo na partial viewu 
            foreach (Utakmica i in sveNaTakmicenju)
            {
                (string tim1, int? rez1, int? rez2, string tim2) par = evidentor.GetPar(i, id);
                UbaciUTabelu(par, ref parovi);
            }
            parovi = parovi.OrderByDescending(x => x.Pobjeda).ToList();
            return PartialView(parovi);
        }

        public void UbaciUTabelu((string tim1, int? rez1, int? rez2, string tim2) par
            , ref List<TabelaStavkaVM> parovi)
        {
            if (par.tim1 != null)
            {
                if (!parovi.Select(x => x.Naziv).Contains(par.tim1))
                    parovi.Add(new TabelaStavkaVM { Naziv = par.tim1, Pobjeda = 0, Poraza = 0, UkupnoOdigrano = 0 });
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
                    parovi.Add(new TabelaStavkaVM { Naziv = par.tim2, Pobjeda = 0, Poraza = 0, UkupnoOdigrano = 0 });
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


        public IActionResult GetFavoriti(int id)
        {
            FavoritiVM model = GetFavPomocna(id);

            return PartialView(model);
        }

        public IActionResult OznaciUtakmicu(int id)
        {
            var userId = db.Users.Where(d => d.Email == User.Identity.Name).FirstOrDefault().Id;

            Favoriti fav = db.Favoriti.Where(d => d.UserID == userId && d.UtakmicaId == id).SingleOrDefault();
            Utakmica u = db.Utakmice.Include(d => d.Runda).ThenInclude(d => d.Bracket).Where(d => d.ID == id).SingleOrDefault();
            if (u != null)
            {
                if (fav != null)
                {
                    db.Remove(fav);
                    db.SaveChanges();
                }
                else
                {
                    Favoriti novi = new Favoriti
                    {
                        UserID = userId,
                        UtakmicaId = id
                    };

                    db.Add(novi);
                    db.SaveChanges();
                }
                FavoritiVM model = GetFavPomocna(u.Runda.Bracket.TakmicenjeID);
                return PartialView("GetFavoriti", model);
            }
            return PartialView("Neuspjeh");
        }

        private FavoritiVM GetFavPomocna(int id)
        {
            List<Utakmica> utakmice = db.Utakmice.AsNoTracking()
                .Include(x => x.UcescaNaUtakmici)
                .Include(x => x.Runda).ThenInclude(x => x.Bracket).ThenInclude(x => x.Takmicenje)
                .Where(x => x.Runda.Bracket.TakmicenjeID == id).ToList();

            FavoritiVM model = new FavoritiVM();
            var userId = db.Users.Where(d => d.Email == User.Identity.Name).FirstOrDefault().Id;

            foreach (var item in utakmice)
            {
                var fav = db.Favoriti.Where(i => i.UserID == userId && i.UtakmicaId == item.ID).SingleOrDefault();

                (string tim1, int? rez1, int? rez2, string tim2) par = evidentor.GetPar(item, id);
                if (fav != null)
                    model.oznaceneUtakmice.Add((par.tim1, par.rez1, par.rez2, par.tim2, item.ID));
                else
                    model.neoznaceneUtakmice.Add((par.tim1, par.rez1, par.rez2, par.tim2, item.ID));
            }

            return model;
        }

        private List<String> GetListaUseraNotifikacije(int utakId)
        {
            List<Favoriti> favoriti = db.Favoriti.Include(d=>d.User).Where(d => d.UtakmicaId == utakId).ToList();
            
            List<String> lista = new List<string>();
            
            foreach (var item in favoriti)
            {
                lista.Add(item.User.Email);
            }
            return lista;
        }
    }
}