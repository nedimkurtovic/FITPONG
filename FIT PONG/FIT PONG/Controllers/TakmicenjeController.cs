using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIT_PONG.Models;
using Microsoft.AspNetCore.Mvc;
using FIT_PONG.ViewModels;
using FIT_PONG.ViewModels.TakmicenjeVMs;
using Microsoft.EntityFrameworkCore;

namespace FIT_PONG.Controllers
{
    public class TakmicenjeController : Controller
    {
        //TODOS
        /*Implementirati dependent dropdownliste sto se tice kategorije takmicenja i vrste takmicenja,ako se odabere mix
         Implementirati za datume,ne moze takmicenje zavrsiti prije pocetka.......*/
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
            if(ModelState.IsValid && !PostojiTakmicenje(objekat.Naziv))
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
            //treba skontat provjeru za editovanje imena,da nije duplikat tj da u tom slucaju nema vise od jednog imena 
            //takvog u bazi ako nisi mijenjao ime takmicenja npr
            if(ModelState.IsValid)
            {
                if(objekat.RokZavrsetkaPrijave.Date.CompareTo(objekat.RokPocetkaPrijave.Date) < 0)
                {
                    return Redirect("/Takmicenje/Neuspjeh");
                }
                Takmicenje obj = db.Takmicenja.Find(objekat.ID);
                if(obj!=null)
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
                        return Redirect("/Takmicenje/Prikaz/" + obj.ID);
                     }
                    catch(DbUpdateException er)
                    {

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
                    ViewBag.takmicenje = obj;
                    return View();
                }
            }
            return Redirect("/Takmicenje/Neuspjeh");
        }
        [HttpPost]
        public IActionResult PotvrdaBrisanja(int ID)
        {
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
                ModelState.AddModelError("", "Doslo je do greške. " + "Pokušajte opet ");

            }
            return View("/Takmicenje/Neuspjeh");
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
        public IActionResult PostojiTakmicenje()
        {
            return View();
        }
    }
}