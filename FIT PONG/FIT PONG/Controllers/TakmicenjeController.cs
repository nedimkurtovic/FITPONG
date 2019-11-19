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
        //HITNO NAC SOLUCIJU ZA VIEWBAGOVE PREVISE NEPOTREBNOG KODA
        public IActionResult Index()
        {
            MyDb db = new MyDb();
            List<TakmicenjeVM> takmicenja = db.Takmicenja.Include(tak=>tak.Kategorija).Include(tak=>tak.Sistem)
                .Include(tak=>tak.Vrsta).Include(tak=>tak.Status).Select(s => new TakmicenjeVM
            (s, db.Prijave.Select(f => f.TakmicenjeID == s.ID).Count())).ToList();
            ViewData["TakmicenjaKey"] = takmicenja;
            return View();
        }
        [HttpPost]
        public IActionResult Dodaj(CreateTakmicenjeVM objekat)
        {
            if(ModelState.IsValid && !PostojiTakmicenje(objekat.Naziv))
            {
                try
                {
                    Takmicenje novo = new Takmicenje(objekat);
                    MyDb db = new MyDb();
                    db.Add(novo);
                    db.SaveChanges();
                    db.Dispose();
                    return Redirect("/Takmicenje/Prikaz/" + novo.ID);
                }
                catch(DbUpdateException er)
                {
                    ModelState.AddModelError("", "Doslo je do greške. " + "Pokušajte opet ");
                }
            }
            MyDb db1 = new MyDb();
            ViewBag.kategorije = db1.Kategorije.Select(s => new ComboBoxVM { ID = s.ID, Opis = s.Opis }).ToList();
            ViewBag.sistemi = db1.SistemiTakmicenja.Select(s => new ComboBoxVM { ID = s.ID, Opis = s.Opis }).ToList();
            ViewBag.vrste = db1.VrsteTakmicenja.Select(s => new ComboBoxVM { ID = s.ID, Opis = s.Naziv }).ToList();
            ViewBag.statusi = db1.StatusiTakmicenja.Select(s => new ComboBoxVM { ID = s.ID, Opis = s.Opis }).ToList();
            db1.Dispose();
            return View(objekat);
        }
        public IActionResult Dodaj()
        {
            MyDb db = new MyDb();
            ViewBag.kategorije = db.Kategorije.Select(s=>new ComboBoxVM {ID=s.ID,Opis=s.Opis }).ToList();
            ViewBag.sistemi = db.SistemiTakmicenja.Select(s => new ComboBoxVM { ID = s.ID, Opis = s.Opis }).ToList();
            ViewBag.vrste = db.VrsteTakmicenja.Select(s => new ComboBoxVM { ID = s.ID, Opis = s.Naziv }).ToList();
            ViewBag.statusi = db.StatusiTakmicenja.Select(s => new ComboBoxVM { ID = s.ID, Opis = s.Opis }).ToList();
            db.Dispose();
            return View();
        }
      
        public IActionResult Prikaz(int? id)
        {
            if(id == null)
            {
                return View("/Takmicenje/Neuspjeh");
            }
            //potreban query za broj rundi,u bracketima se nalazi takmicenjeID ,bar bi trebalo opotrebna migracija
            MyDb db = new MyDb();
            Takmicenje obj = db.Takmicenja.Include(tak => tak.Kategorija).Include(tak => tak.Sistem)
                .Include(tak => tak.Vrsta).Include(tak => tak.Status).SingleOrDefault(y=> y.ID == id);
            if (obj != null)
            {
                TakmicenjeVM takmicenje = new TakmicenjeVM(obj);
                ViewData["takmicenjeKey"] = takmicenje;
                return View();
            }
            return Redirect("/Takmicenje/Neuspjeh");
        }
        public IActionResult Edit(int id)
        {
            MyDb db = new MyDb();
            Takmicenje obj = db.Takmicenja.Find(id);
            if(obj != null)
            {
                EditTakmicenjeVM ob1 = new EditTakmicenjeVM(obj);
                ViewBag.kategorije = db.Kategorije.Select(s => new ComboBoxVM { ID = s.ID, Opis = s.Opis }).ToList();
                ViewBag.sistemi = db.SistemiTakmicenja.Select(s => new ComboBoxVM { ID = s.ID, Opis = s.Opis }).ToList();
                ViewBag.vrste = db.VrsteTakmicenja.Select(s => new ComboBoxVM { ID = s.ID, Opis = s.Naziv }).ToList();
                ViewBag.statusi = db.StatusiTakmicenja.Select(s => new ComboBoxVM { ID = s.ID, Opis = s.Opis }).ToList();
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
                MyDb db = new MyDb();
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
                        db.Dispose();
                        return Redirect("/Takmicenje/Prikaz/" + obj.ID);
                     }
                    catch(DbUpdateException er)
                    {

                    }
                }
            }
            MyDb db1 = new MyDb();
            ViewBag.kategorije = db1.Kategorije.Select(s => new ComboBoxVM { ID = s.ID, Opis = s.Opis }).ToList();
            ViewBag.sistemi = db1.SistemiTakmicenja.Select(s => new ComboBoxVM { ID = s.ID, Opis = s.Opis }).ToList();
            ViewBag.vrste = db1.VrsteTakmicenja.Select(s => new ComboBoxVM { ID = s.ID, Opis = s.Naziv }).ToList();
            ViewBag.statusi = db1.StatusiTakmicenja.Select(s => new ComboBoxVM { ID = s.ID, Opis = s.Opis }).ToList();
            db1.Dispose();
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
                MyDb db = new MyDb();
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
                MyDb db = new MyDb();
                Takmicenje obj = db.Takmicenja.Find(ID);
                db.Remove(obj);
                db.SaveChanges();
                db.Dispose();
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
            MyDb db = new MyDb();
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