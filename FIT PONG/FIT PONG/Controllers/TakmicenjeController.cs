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
            List<ComboBoxVM> kategorije = db.Kategorije.Select(s=>new ComboBoxVM {ID=s.ID,Opis=s.Opis }).ToList();
            List<ComboBoxVM> sistemi = db.SistemiTakmicenja.Select(s => new ComboBoxVM { ID = s.ID, Opis = s.Opis }).ToList();
            List<ComboBoxVM> vrste = db.VrsteTakmicenja.Select(s => new ComboBoxVM { ID = s.ID, Opis = s.Naziv }).ToList();
            List<ComboBoxVM> statusi = db.StatusiTakmicenja.Select(s => new ComboBoxVM { ID = s.ID, Opis = s.Opis }).ToList();
            ViewData["KategorijeKey"] = kategorije;
            ViewData["SistemiKey"] = sistemi;
            ViewData["VrsteKey"] = vrste;
            ViewData["StatusiKey"] = statusi;
            return View();
        }
        public IActionResult Snimi(string _naziv,DateTime _pocetakprijava,DateTime _krajprijava,
            int _minimalniELO,int _kategorijaID,int _sistemID,int _vrstaID,int _statusID, DateTime? _pocetaktakmicenja = null)
        {
            Takmicenje novo = new Takmicenje(_naziv,_pocetakprijava,_krajprijava,_minimalniELO,_kategorijaID,
                _sistemID,_vrstaID,_statusID,_pocetaktakmicenja.GetValueOrDefault(DateTime.Parse("1 Jan 1900")));
            if(ModelState.IsValid)//ako ima problema s bindanjem javit ce to ovdje i nece proci u dodavanje za provjeru koristi anotacije na modelu
            {
                MyDb db = new MyDb();
                db.Takmicenja.Add(novo);
                db.SaveChanges();
                //int id = db.Takmicenja.Last().ID;
                db.Dispose();
                //realno ovdje cu vratiti prikaz pojedinacnog takmicenja pomocu akcije prikaz/{id} ali privremeno moze
                //ovako
                return Redirect("/Takmicenje/Prikaz/?id=" + novo.ID);
            }
            return Redirect("/Takmicenje/Neuspjeh");
        }
        public IActionResult Prikaz(int id)
        {
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
                //SPAGHETTII CODE NE VALja
                //mora biti neki drugi nacin ovo je katastrofa,ili barem izdvojit u zasebun funkciju pa skontat TODO!!!
                List<ComboBoxVM> kategorije = db.Kategorije.Select(s => new ComboBoxVM { ID = s.ID, Opis = s.Opis }).ToList();
                List<ComboBoxVM> sistemi = db.SistemiTakmicenja.Select(s => new ComboBoxVM { ID = s.ID, Opis = s.Opis }).ToList();
                List<ComboBoxVM> vrste = db.VrsteTakmicenja.Select(s => new ComboBoxVM { ID = s.ID, Opis = s.Naziv }).ToList();
                List<ComboBoxVM> statusi = db.StatusiTakmicenja.Select(s => new ComboBoxVM { ID = s.ID, Opis = s.Opis }).ToList();
                ViewData["KategorijeKey"] = kategorije;
                ViewData["SistemiKey"] = sistemi;
                ViewData["VrsteKey"] = vrste;
                ViewData["StatusiKey"] = statusi;
                return View();
            }
            return Redirect("/Takmicenje/Neuspjeh");
        }
        public IActionResult Edit(int id, string _naziv, DateTime _pocetakprijava, DateTime _krajprijava,
            int _minimalniELO, int _kategorijaID, int _sistemID, int _vrstaID, int _statusID,
           DateTime? _pocetaktakmicenja = null, DateTime ? _zavrsetakTakmicenja=null)
        {
            if(ModelState.IsValid)
            {
                MyDb db = new MyDb();
                Takmicenje obj = db.Takmicenja.Find(id);
                if (obj != null)
                {
                    obj.setAtribute(_naziv, _pocetakprijava, _krajprijava, _minimalniELO, _kategorijaID,
                    _sistemID, _vrstaID, _statusID, _zavrsetakTakmicenja.GetValueOrDefault(),
                    _pocetaktakmicenja.GetValueOrDefault(DateTime.Parse("1 Jan 1900")));
                    db.SaveChanges();
                    db.Dispose();
                    return Redirect("/Takmicenje/Prikaz/?id=" + id);
                }
            }
            return Redirect("/Takmicenje/Neuspjeh");
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