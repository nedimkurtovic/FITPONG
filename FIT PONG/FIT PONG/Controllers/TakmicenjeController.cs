using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIT_PONG.Models;
using Microsoft.AspNetCore.Mvc;
using FIT_PONG.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FIT_PONG.Controllers
{
    public class TakmicenjeController : Controller
    {
        public IActionResult Index()
        {
            MyDb db = new MyDb();
            List<TakmicenjeVM> takmicenja = db.Takmicenja.Include(tak=>tak.Kategorija).Include(tak=>tak.Sistem)
                .Include(tak=>tak.Vrsta).Include(tak=>tak.Status).Select(s => new TakmicenjeVM
            (s, db.Prijave.Select(f => f.TakmicenjeID == s.ID).Count())).ToList();
            ViewData["TakmicenjaKey"] = takmicenja;
            return View();
        }
        public IActionResult Dodaj()
        {
            MyDb db = new MyDb();
            ViewBag.kategorije = db.Kategorije.Select(s=>new ComboBoxVM {ID=s.ID,Opis=s.Opis }).ToList();
            ViewBag.sistemi = db.SistemiTakmicenja.Select(s => new ComboBoxVM { ID = s.ID, Opis = s.Opis }).ToList();
            ViewBag.vrste = db.VrsteTakmicenja.Select(s => new ComboBoxVM { ID = s.ID, Opis = s.Naziv }).ToList();
            ViewBag.statusi = db.StatusiTakmicenja.Select(s => new ComboBoxVM { ID = s.ID, Opis = s.Opis }).ToList();

            return View();
        }
        [HttpPost]
        public IActionResult Snimi(string _naziv,DateTime _pocetakprijava,DateTime _krajprijava,
                                   int _minimalniELO,int _kategorijaID,int _sistemID,
                                   int _vrstaID,int _statusID, DateTime? _pocetaktakmicenja = null)

        {

            Takmicenje novo = new Takmicenje(_naziv,_pocetakprijava,_krajprijava,_minimalniELO,_kategorijaID,
                _sistemID,_vrstaID,_statusID,_pocetaktakmicenja);
  
            try { 
                MyDb db = new MyDb();
                db.Takmicenja.Add(novo);
                db.SaveChanges();
                //int id = db.Takmicenja.Last().ID;
                db.Dispose();
                //realno ovdje cu vratiti prikaz pojedinacnog takmicenja pomocu akcije prikaz/{id} ali privremeno moze
                //ovako
                return Redirect("/Takmicenje/Prikaz/?id=" + novo.ID);
            }
            catch(DbUpdateException err)
            {
                ModelState.AddModelError("", "Doslo je do greške. " + "Pokušajte opet ");
            }
            return Redirect("/Takmicenje/Neuspjeh");
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
            Takmicenje obj = db.Takmicenja.Include(tak => tak.Kategorija).Include(tak => tak.Sistem)
                .Include(tak => tak.Vrsta).Include(tak => tak.Status).SingleOrDefault(y => y.ID == id);
            if (obj != null)
            {
                ViewBag.kategorije = db.Kategorije.Select(s => new ComboBoxVM { ID = s.ID, Opis = s.Opis }).ToList();
                ViewBag.sistemi = db.SistemiTakmicenja.Select(s => new ComboBoxVM { ID = s.ID, Opis = s.Opis }).ToList();
                ViewBag.vrste = db.VrsteTakmicenja.Select(s => new ComboBoxVM { ID = s.ID, Opis = s.Naziv }).ToList();
                ViewBag.statusi = db.StatusiTakmicenja.Select(s => new ComboBoxVM { ID = s.ID, Opis = s.Opis }).ToList();
                ViewBag.takmicenje = obj;
                return View();
            }
            return Redirect("/Takmicenje/Neuspjeh");
        }
        [HttpPost]
        public IActionResult Edit(int id, string _naziv, DateTime _pocetakprijava, DateTime _krajprijava,
            int _minimalniELO, int _kategorijaID, int _sistemID, int _vrstaID, int _statusID,
           DateTime? _pocetaktakmicenja = null, DateTime? _zavrsetakTakmicenja = null)
        {
            MyDb db = new MyDb();
            Takmicenje obj = db.Takmicenja.Find(id);
            if (obj != null) 
            {
                try {
                    obj.setAtribute(_naziv, _pocetakprijava, _krajprijava, _minimalniELO, _kategorijaID,
                    _sistemID, _vrstaID, _statusID, _zavrsetakTakmicenja.GetValueOrDefault(),
                    _pocetaktakmicenja.GetValueOrDefault());
                    db.Takmicenja.Update(obj);
                    db.SaveChanges();
                    db.Dispose();
                    return Redirect("/Takmicenje/Prikaz/?id=" + id);
                }
                catch (DbUpdateException err)
                {
                    ModelState.AddModelError("", "Doslo je do greške. " + "Pokušajte opet ");
                }
            }

            return Redirect("/Takmicenje/Neuspjeh");
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
                return View("Index);
            }
            catch (DbUpdateException err)
            {
                ModelState.AddModelError("", "Doslo je do greške. " + "Pokušajte opet ");

            }
            return View("/Takmicenje/Neuspjeh");
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