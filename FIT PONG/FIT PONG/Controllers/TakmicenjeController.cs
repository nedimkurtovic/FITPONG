using FIT_PONG.Models;
using FIT_PONG.ViewModels;
using FIT_PONG.ViewModels.TakmicenjeVMs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIT_PONG.Controllers
{
    public class TakmicenjeController : Controller
    {
        //TODOS
        /*Implementirati dependent dropdownliste sto se tice kategorije takmicenja i vrste takmicenja,ako se odabere mix
         Implementirati za datume,ne moze takmicenje zavrsiti prije pocetka.......
         Pocetak takmicenja ne moze biti prije zavrsetka roka prijava
         
           Konkretan problem za validacijsku poruku : Posto je koristen html helper on dodaje defaultnu vrijednost ali 
         nema mogucnost da je disablea tj da primora korisnika da submitta npr kategoriju ili sistem i onda sta se desava 
         kad korisnik submitta npr Odaberite Kategoriju to se ovdje binda kao "" i iz nekog razloga ne dopusta mmoju errormessage :(
         isto vrijedi i za datepicker,potrebno naci soluciju hint(nullable sve)*/
        private readonly MyDb db;
        public TakmicenjeController(MyDb instanca)
        {
            db = instanca;
        }
        public IActionResult Index()
        {
            List<TakmicenjeVM> takmicenja = db.Takmicenja.Include(tak=>tak.Kategorija).Include(tak=>tak.Sistem)
                .Include(tak=>tak.Vrsta).Include(tak=>tak.Status).Include(tak=>tak.Feed).Select(s => new TakmicenjeVM
            (s, db.Prijave.Select(f => f.TakmicenjeID == s.ID).Count())).ToList();
            ViewData["TakmicenjaKey"] = takmicenja;
            return View();
        }
        [HttpPost]
        public IActionResult Dodaj(CreateTakmicenjeVM objekat)
        {
            if(ModelState.IsValid)
            {
                if (PostojiTakmicenje(objekat.Naziv))
                    ModelState.AddModelError("", "Vec postoji takmicenje u bazi");
                if (objekat.RokZavrsetkaPrijave.CompareTo(objekat.RokPocetkaPrijave) < 0)
                    ModelState.AddModelError(nameof(objekat.RokZavrsetkaPrijave), "Datum zavrsetka prijava ne moze biti prije pocetka");
                if (objekat.DatumPocetka != null && objekat.DatumPocetka < objekat.RokZavrsetkaPrijave)
                    ModelState.AddModelError(nameof(objekat.DatumPocetka), "Datum pocetka ne moze biti prije zavrsetka prijava");
                if(ModelState.ErrorCount == 0)
                {
                    using (var transakcija = db.Database.BeginTransaction())//sigurnost u opasnim situacijama 
                    {
                        try
                        {
                            Takmicenje novo = new Takmicenje(objekat);
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
            if(id == null)
            {
                return View("/Takmicenje/Neuspjeh");
            }
            //potreban query za broj rundi,u bracketima se nalazi takmicenjeID ,bar bi trebalo opotrebna migracija
            Takmicenje obj = db.Takmicenja.Include(tak => tak.Kategorija).Include(tak => tak.Sistem)
                .Include(tak => tak.Vrsta).Include(tak => tak.Status).Include(tak=>tak.Feed).SingleOrDefault(y=> y.ID == id);
            if (obj != null)
            {
                TakmicenjeVM takmicenje = new TakmicenjeVM(obj);
                return View(takmicenje);
            }
            return Redirect("/Takmicenje/Neuspjeh");
        }

        public IActionResult Edit(int id)
        {
            Takmicenje obj = db.Takmicenja.Find(id);
            if(obj != null)
            {
                EditTakmicenjeVM ob1 = new EditTakmicenjeVM(obj);
                LoadViewBag();
                return View(ob1);
            }
            return Redirect("/Takmicenje/Neuspjeh");
        }
        [HttpPost]
        public IActionResult Edit(EditTakmicenjeVM objekat)
        {
            if(ModelState.IsValid)
            {
                if (TakmicenjaViseOd(objekat.Naziv,objekat.ID))
                    ModelState.AddModelError(nameof(objekat.Naziv), "Vec postoji takmicenje u bazi");
                if (objekat.RokZavrsetkaPrijave.CompareTo(objekat.RokPocetkaPrijave) < 0)
                    ModelState.AddModelError(nameof(objekat.RokZavrsetkaPrijave), "Datum zavrsetka prijava ne moze biti prije pocetka");
                if (objekat.DatumPocetka != null && objekat.DatumPocetka < objekat.RokZavrsetkaPrijave)
                    ModelState.AddModelError(nameof(objekat.DatumPocetka), "Datum pocetka ne moze biti prije zavrsetka prijava");
                if (objekat.DatumPocetka != null && objekat.DatumZavrsetka != null && objekat.DatumZavrsetka < objekat.DatumPocetka)
                    ModelState.AddModelError(nameof(objekat.DatumZavrsetka), "Datum pocetka takmicenja ne moze biti prije zavrsetka");
                if (ModelState.ErrorCount == 0)
                {
                    Takmicenje obj = db.Takmicenja.Find(objekat.ID);
                    if (obj != null)
                    {
                        using (var transakcija = db.Database.BeginTransaction())
                        {
                            try
                            {
                                obj.Naziv = objekat.Naziv;
                                obj.DatumPocetka = objekat.DatumPocetka ?? null;
                                obj.DatumZavrsetka = objekat.DatumZavrsetka ?? null;
                                obj.RokPocetkaPrijave = objekat.RokPocetkaPrijave;
                                obj.RokZavrsetkaPrijave = objekat.RokZavrsetkaPrijave;
                                obj.MinimalniELO = objekat.MinimalniELO;
                                obj.KategorijaID = objekat.KategorijaID;
                                obj.VrstaID = objekat.VrstaID;
                                obj.StatusID = objekat.StatusID;
                                db.Update(obj);
                                db.SaveChanges();

                                //smells fishy here,mada se teoretski ne bi trebalo desiti da ne postoji fid za dato takmicenje 
                                //ali nesto smells fishy,ja bravo : ako se desi da se obrisao fid iz nekog razloga u bazi nakon kreiranja takmicenja
                                //mozda bi bilo pametno provjeriti ovdje da li je null mada imam transakciju da me iscupa iz problema
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
            }
            LoadViewBag();
            return View(objekat);
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
            return View("/Takmicenje/Neuspjeh");
        }
        public bool TakmicenjaViseOd(string naziv,int ID)
        {
            if (db.Takmicenja.Where(s => s.Naziv == naziv && s.ID != ID).Count() > 0)
                return true;
            return false;
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
    }
}